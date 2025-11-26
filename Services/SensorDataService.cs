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

        public SensorDataService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        // --- Métodos de Configuración (llamados desde Monitoreo.razor) ---
        public void SetCurrentEscenario(int id) => _currentEscenarioId = id;
        public void SetCurrentSensor(int id) => _currentSensorId = id;
        public void SetCurrentUser(int id) => _currentUserId = id;

        public string[] GetAvailablePorts() => SerialPort.GetPortNames();

        // --- LÓGICA PRINCIPAL DE PROCESAMIENTO Y GUARDADO ---

        private async Task ProcesarYGuardar(LecturaArduinoDto data)
        {
            // 1. Validar que tengamos configuración seleccionada
            if (_currentEscenarioId == 0 || _currentSensorId == 0 || _currentUserId == 0)
            {
                Debug.WriteLine("Datos recibidos pero NO guardados: Falta seleccionar Escenario, Sensor o Usuario.");
                return;
            }

            // Usamos un Scope nuevo para operaciones de base de datos en segundo plano
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // 2. Obtener qué parámetros tiene configurados este sensor en la BD
                // (Ej: Si el sensor tiene pH y Temperatura, solo guardaremos esos)
                var parametrosDelSensor = await db.Parametros
                    .Where(p => p.sensor_id == _currentSensorId)
                    .ToListAsync();

                if (!parametrosDelSensor.Any()) return;

                var fechaActual = DateTime.Now; // Usamos hora local del servidor/PC

                // 3. Llamar a métodos individuales solo si el parámetro existe para este sensor

                // --- pH ---
                var paramPH = parametrosDelSensor.FirstOrDefault(p => p.nombre_parametro.Equals("pH", StringComparison.OrdinalIgnoreCase));
                if (paramPH != null)
                {
                    GuardarPH(db, data, paramPH.parametro_id, fechaActual);
                }

                // --- Turbidez ---
                var paramTurb = parametrosDelSensor.FirstOrDefault(p => p.nombre_parametro.Equals("Turbidez", StringComparison.OrdinalIgnoreCase));
                if (paramTurb != null)
                {
                    GuardarTurbidez(db, data, paramTurb.parametro_id, fechaActual);
                }

                // --- Temperatura ---
                var paramTemp = parametrosDelSensor.FirstOrDefault(p => p.nombre_parametro.Equals("Temperatura", StringComparison.OrdinalIgnoreCase));
                if (paramTemp != null)
                {
                    GuardarTemperatura(db, data, paramTemp.parametro_id, fechaActual);
                }

                // --- Conductividad ---
                var paramCond = parametrosDelSensor.FirstOrDefault(p => p.nombre_parametro.Equals("Conductividad", StringComparison.OrdinalIgnoreCase));
                if (paramCond != null)
                {
                    GuardarConductividad(db, data, paramCond.parametro_id, fechaActual);
                }

                // 4. Confirmar cambios en la base de datos
                if (db.ChangeTracker.HasChanges())
                {
                    await db.SaveChangesAsync();
                    Debug.WriteLine("Mediciones guardadas correctamente en la BD.");
                }
            }
        }

        // --- MÉTODOS INDIVIDUALES DE GUARDADO ---

        private void GuardarPH(ApplicationDbContext db, LecturaArduinoDto data, int parametroId, DateTime fecha)
        {
            db.Mediciones.Add(new Medicion
            {
                escenario_id = _currentEscenarioId,
                sensor_id = _currentSensorId,
                user_id = _currentUserId,
                created_by = _currentUserId,
                parametro_id = parametroId,

                fecha_hora = fecha,
                created_at = fecha,
                updated_at = fecha,

                // Mapeo usando las propiedades del DTO (LecturaArduinoDto)
                valor_analogico = (double)data.PH,
                valor_cv_decimal = (decimal)data.PH_CV,
                status = (int)MapStatus(data.PH_Status),

                valor_digital = 0 // Valor por defecto si no se usa
            });
        }

        private void GuardarTurbidez(ApplicationDbContext db, LecturaArduinoDto data, int parametroId, DateTime fecha)
        {
            db.Mediciones.Add(new Medicion
            {
                escenario_id = _currentEscenarioId,
                sensor_id = _currentSensorId,
                user_id = _currentUserId,
                created_by = _currentUserId,
                parametro_id = parametroId,

                fecha_hora = fecha,
                created_at = fecha,
                updated_at = fecha,

                valor_analogico = (double)data.Turbidez_NTU,
                valor_cv_decimal = (decimal)data.Turbidez_CV,
                status = (int)MapStatus(data.Turbidez_Status),

                valor_digital = 0
            });
        }

        private void GuardarTemperatura(ApplicationDbContext db, LecturaArduinoDto data, int parametroId, DateTime fecha)
        {
            db.Mediciones.Add(new Medicion
            {
                escenario_id = _currentEscenarioId,
                sensor_id = _currentSensorId,
                user_id = _currentUserId,
                created_by = _currentUserId,
                parametro_id = parametroId,

                fecha_hora = fecha,
                created_at = fecha,
                updated_at = fecha,

                valor_analogico = (double)data.Temperatura_C,
                valor_cv_decimal = (decimal)data.Temperatura_CV,
                status = (int)StatusMedicion.Ideal, // Temperatura siempre suele ser 'Ideal' o no aplica norma estricta igual que pH

                valor_digital = 0
            });
        }

        private void GuardarConductividad(ApplicationDbContext db, LecturaArduinoDto data, int parametroId, DateTime fecha)
        {
            db.Mediciones.Add(new Medicion
            {
                escenario_id = _currentEscenarioId,
                sensor_id = _currentSensorId,
                user_id = _currentUserId,
                created_by = _currentUserId,
                parametro_id = parametroId,

                fecha_hora = fecha,
                created_at = fecha,
                updated_at = fecha,

                valor_analogico = (double)data.Conductividad_uScm,
                valor_cv_decimal = (decimal)data.Conductividad_CV,
                status = (int)StatusMedicion.Ideal,

                valor_digital = 0
            });
        }

        // --- LÓGICA DE CONEXIÓN SERIAL Y PARSEO JSON ---

        public bool StartListening(string portName)
        {
            try
            {
                StopListening(); // Cerrar si había uno abierto
                _serialPort = new SerialPort(portName, 9600);
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al abrir puerto: {ex.Message}");
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

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                // Leemos lo que haya en el buffer
                string data = sp.ReadExisting();
                _jsonBuffer += data;

                // Procesamos línea por línea
                string? line;
                while ((line = ExtractLine()) != null)
                {
                    ProcessJson(line);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error lectura serial: {ex.Message}");
            }
        }

        // Extrae una línea completa del buffer acumulado
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
            // Validación básica de inicio de JSON
            if (!json.StartsWith("{")) return;

            try
            {
                // Deserializamos usando el DTO que coincide con Arduino
                var data = JsonSerializer.Deserialize<LecturaArduinoDto>(json);

                if (data != null)
                {
                    // 1. Notificar a la UI (Gráficas, Tablas en tiempo real)
                    OnDataReceived?.Invoke(data);

                    // 2. Lanzar tarea de guardado en BD (Fuego y olvido)
                    Task.Run(() => ProcesarYGuardar(data));
                }
            }
            catch (JsonException)
            {
                // Ignorar líneas incompletas o corruptas
                Debug.WriteLine("JSON incompleto o inválido recibido.");
            }
        }

        // Convierte el string del Arduino al Enum de C#
        private StatusMedicion MapStatus(string? s) => s?.ToLower() switch
        {
            "ideal" => StatusMedicion.Ideal,
            "apta" => StatusMedicion.Apta,
            "cumple" => StatusMedicion.Apta,
            "no apta" => StatusMedicion.NoApta,
            "no_apto" => StatusMedicion.NoApta,
            "fuera_norma" => StatusMedicion.NoApta,
            _ => StatusMedicion.Ideal // Valor por defecto
        };

        public void Dispose()
        {
            StopListening();
        }
    }
}