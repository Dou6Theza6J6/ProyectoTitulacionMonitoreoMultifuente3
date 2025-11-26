using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonitoreoMultifuente3.Data;
using MonitoreoMultifuente3.DTOs;
using MonitoreoMultifuente3.Enums;
using MonitoreoMultifuente3.Models;

namespace MonitoreoMultifuente3.Services
{
    public class SensorDataService : IDisposable
    {
        private SerialPort? _serialPort;
        private readonly IServiceScopeFactory _scopeFactory;
        private string _jsonBuffer = string.Empty;

        // Variables de configuración seleccionadas en la interfaz
        private int _currentEscenarioId = 0;
        private int _currentSensorId = 0;
        private int _currentUserId = 0;

        // Evento para enviar datos a la UI en tiempo real
        public event Action<LecturaArduinoDto>? OnDataReceived;

        // Propiedades para saber el estado desde la UI
        public bool IsConnected => _serialPort != null && _serialPort.IsOpen;
        public string ConnectedPortName => _serialPort?.PortName ?? "";

        public SensorDataService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        // --- Métodos de Configuración (llamados desde Monitoreo.razor) ---
        public void SetCurrentEscenario(int id) => _currentEscenarioId = id;
        public void SetCurrentSensor(int id) => _currentSensorId = id;
        public void SetCurrentUser(int id) => _currentUserId = id;
        public string[] GetAvailablePorts() => SerialPort.GetPortNames();

        // ==================================================================================
        //  NUEVO: DETECCIÓN AUTOMÁTICA DE ARDUINO
        // ==================================================================================
        public async Task<bool> AutoConectar()
        {
            // 1. Cerramos cualquier conexión previa
            StopListening();

            var puertos = SerialPort.GetPortNames();
            if (puertos.Length == 0) return false;

            Debug.WriteLine($"Iniciando autoconexión... Puertos visibles: {string.Join(", ", puertos)}");

            // 2. Probamos puerto por puerto
            foreach (var puerto in puertos)
            {
                try
                {
                    Debug.WriteLine($"Probando puerto {puerto}...");
                    using (var puertoPrueba = new SerialPort(puerto, 9600))
                    {
                        puertoPrueba.ReadTimeout = 2500; // Esperar máx 2.5 segundos
                        puertoPrueba.Open();

                        // Esperamos un poco a que el Arduino se reinicie al abrir el puerto
                        await Task.Delay(2000);

                        // Leemos lo que haya en el buffer
                        string datos = puertoPrueba.ReadExisting();
                        Debug.WriteLine($"Datos recibidos en {puerto}: {datos}");

                        // 3. Verificamos si es un JSON válido de NUESTRO Arduino
                        // Buscamos claves únicas como "pH" o estructura JSON
                        if (datos.Contains("\"pH\"") || (datos.Contains("{") && datos.Contains("}")))
                        {
                            Debug.WriteLine($"¡Arduino detectado en {puerto}!");
                            puertoPrueba.Close(); // Cerramos la prueba

                            // 4. Abrimos la conexión oficial
                            return StartListening(puerto);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Fallo al probar {puerto}: {ex.Message}");
                    // Continuamos con el siguiente puerto...
                }
            }
            return false;
        }

        // --- CONEXIÓN MANUAL ---
        public bool StartListening(string portName)
        {
            try
            {
                StopListening(); // Seguridad
                _serialPort = new SerialPort(portName, 9600);
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fatal abriendo puerto: {ex.Message}");
                return false;
            }
        }

        public void StopListening()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.DataReceived -= SerialPort_DataReceived;
                _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;
            }
        }

        // --- LECTURA DE DATOS SERIALES ---
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string data = sp.ReadExisting();
                _jsonBuffer += data;

                string? line;
                // Procesamos línea por línea para asegurar JSONs completos
                while ((line = ExtractLine()) != null)
                {
                    ProcessJson(line);
                }
            }
            catch (Exception ex) { Debug.WriteLine($"Error lectura serial: {ex.Message}"); }
        }

        private string? ExtractLine()
        {
            int idx = _jsonBuffer.IndexOf('\n');
            if (idx == -1) return null;

            string line = _jsonBuffer.Substring(0, idx).Trim();
            _jsonBuffer = _jsonBuffer.Substring(idx + 1);
            return line;
        }

        private void ProcessJson(string json)
        {
            if (!json.StartsWith("{")) return; // Validación rápida

            try
            {
                var data = JsonSerializer.Deserialize<LecturaArduinoDto>(json);
                if (data != null)
                {
                    // 1. Enviar a la UI (Gráficas/Tarjetas en vivo)
                    OnDataReceived?.Invoke(data);

                    // 2. Guardar en BD en segundo plano (si corresponde)
                    Task.Run(() => ProcesarYGuardar(data));
                }
            }
            catch (JsonException) { Debug.WriteLine("JSON incompleto o corrupto ignorado."); }
        }

        // --- LÓGICA DE GUARDADO EN BD ---
        private async Task ProcesarYGuardar(LecturaArduinoDto data)
        {
            // REGLA DE ORO: Solo guardamos si el usuario seleccionó un Escenario, un Usuario y un Sensor ESPECÍFICO.
            // Si _currentSensorId es 0 (Todos), NO guardamos, solo visualizamos.
            if (_currentEscenarioId == 0 || _currentSensorId == 0 || _currentUserId == 0) return;

            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Obtenemos los parámetros que TIENE este sensor en la base de datos
                var parametros = await db.Parametros
                    .Where(p => p.sensor_id == _currentSensorId)
                    .AsNoTracking()
                    .ToListAsync();

                if (!parametros.Any()) return;

                var fecha = DateTime.Now;

                // Mapeamos el JSON a los parámetros de la BD por nombre
                // (pH -> data.PH, Turbidez -> data.Turbidez_NTU, etc.)

                var pPh = parametros.FirstOrDefault(p => p.nombre_parametro.Equals("pH", StringComparison.OrdinalIgnoreCase));
                if (pPh != null) GuardarMedicion(db, pPh.parametro_id, data.PH, data.PH_CV, data.PH_Status, fecha);

                var pTurb = parametros.FirstOrDefault(p => p.nombre_parametro.Equals("Turbidez", StringComparison.OrdinalIgnoreCase));
                if (pTurb != null) GuardarMedicion(db, pTurb.parametro_id, data.Turbidez_NTU, data.Turbidez_CV, data.Turbidez_Status, fecha);

                var pTemp = parametros.FirstOrDefault(p => p.nombre_parametro.Equals("Temperatura", StringComparison.OrdinalIgnoreCase));
                if (pTemp != null) GuardarMedicion(db, pTemp.parametro_id, data.Temperatura_C, data.Temperatura_CV, "Ideal", fecha);

                var pCond = parametros.FirstOrDefault(p => p.nombre_parametro.Equals("Conductividad", StringComparison.OrdinalIgnoreCase));
                if (pCond != null) GuardarMedicion(db, pCond.parametro_id, data.Conductividad_uScm, data.Conductividad_CV, "Ideal", fecha);

                // Guardar cambios si hubo alguno
                if (db.ChangeTracker.HasChanges())
                {
                    await db.SaveChangesAsync();
                    Debug.WriteLine("Datos guardados exitosamente en la BD.");
                }
            }
        }

        private void GuardarMedicion(ApplicationDbContext db, int paramId, float valor, float cv, string statusStr, DateTime fecha)
        {
            db.Mediciones.Add(new Medicion
            {
                escenario_id = _currentEscenarioId,
                sensor_id = _currentSensorId,
                user_id = _currentUserId,
                created_by = _currentUserId,
                parametro_id = paramId,
                fecha_hora = fecha,
                created_at = fecha,
                updated_at = fecha,
                valor_analogico = (double)valor,
                valor_cv_decimal = (decimal)cv,
                status = (int)MapStatus(statusStr),
                valor_digital = 0
            });
        }

        private StatusMedicion MapStatus(string? s) => s?.ToLower() switch
        {
            "ideal" => StatusMedicion.Ideal,
            "apta" => StatusMedicion.Apta,
            "cumple" => StatusMedicion.Apta,
            "no apta" => StatusMedicion.NoApta,
            "no_apto" => StatusMedicion.NoApta,
            "fuera_norma" => StatusMedicion.NoApta,
            _ => StatusMedicion.Ideal
        };

        public void Dispose()
        {
            StopListening();
        }
    }
}