using System.IO.Ports;
using System.Text.Json;
using System.Diagnostics;
using MonitoreoMultifuente3.Data;
using MonitoreoMultifuente3.Models;
using MonitoreoMultifuente3.DTOs;
using MonitoreoMultifuente3.Enums; // <--- Importante para StatusMedicion
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // --- LÓGICA PRINCIPAL DE PROCESAMIENTO ---

        // Este método orquesta el guardado llamando a las funciones individuales
        private async Task ProcesarYGuardar(LecturaArduinoDto data)
        {
            // 1. Validar que tengamos configuración
            if (_currentEscenarioId == 0 || _currentSensorId == 0 || _currentUserId == 0)
            {
                Debug.WriteLine("Datos recibidos pero NO guardados: Falta seleccionar Escenario, Sensor o Usuario.");
                return;
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // 2. Obtener parámetros configurados para el sensor actual en la BD
                var parametrosDelSensor = await db.Parametros
                    .Where(p => p.sensor_id == _currentSensorId)
                    .ToListAsync();

                if (!parametrosDelSensor.Any()) return;

                var fechaActual = DateTime.UtcNow;

                // 3. Llamar a métodos individuales si el parámetro existe en la BD

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
                    Debug.WriteLine("Mediciones guardadas correctamente por métodos individuales.");
                }
            }
        }

        // --- MÉTODOS INDIVIDUALES DE GUARDADO ---
        // Cada uno se encarga de crear el objeto Medicion con los datos correctos

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

                // CORRECCIÓN: Usamos los nombres exactos de tu DTO (PascalCase)
                valor_analogico = (double)data.PH,
                valor_cv_decimal = (decimal)data.PhCV,
                status = (int)MapStatus(data.PhStatus)
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

                valor_analogico = (double)data.TurbidezNTU,
                valor_cv_decimal = (decimal)data.TurbidezCV,
                status = (int)MapStatus(data.TurbidezStatus)
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

                valor_analogico = (double)data.TemperaturaC,
                valor_cv_decimal = (decimal)data.TemperaturaCV,
                status = (int)StatusMedicion.Ideal
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

                valor_analogico = (double)data.ConductividadUsScm,
                valor_cv_decimal = (decimal)data.ConductividadCV,
                status = (int)StatusMedicion.Ideal
            });
        }

        // --- LÓGICA DE CONEXIÓN Y PARSEO JSON ---

        public bool StartListening(string portName)
        {
            try
            {
                StopListening();
                _serialPort = new SerialPort(portName, 9600);
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error puerto: {ex.Message}");
                return false;
            }
        }

        public void StopListening()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                _jsonBuffer += ((SerialPort)sender).ReadExisting();
                string? line;
                while ((line = ExtractLine()) != null)
                {
                    ProcessJson(line);
                }
            }
            catch (Exception ex) { Debug.WriteLine($"Error lectura: {ex.Message}"); }
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
            if (!json.StartsWith("{")) return;

            try
            {
                var data = JsonSerializer.Deserialize<LecturaArduinoDto>(json);
                if (data != null)
                {
                    // Notificar UI
                    OnDataReceived?.Invoke(data);

                    // Lanzar tarea de guardado
                    Task.Run(() => ProcesarYGuardar(data));
                }
            }
            catch (JsonException) { /* Ignorar JSON incompleto */ }
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

        public void Dispose() => StopListening();
    }
}