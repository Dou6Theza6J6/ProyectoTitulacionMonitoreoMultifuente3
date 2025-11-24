using System.IO.Ports;
using System.Text.Json;
using System.Diagnostics;
using MonitoreoMultifuente3.Data;
using MonitoreoMultifuente3.Models;
using MonitoreoMultifuente3.DTOs;
using MonitoreoMultifuente3.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MonitoreoMultifuente3.Services
{
    public class SensorDataService : IDisposable
    {
        private SerialPort? _serialPort;
        private readonly IServiceScopeFactory _scopeFactory;
        private string _jsonBuffer = string.Empty;

        // --- Variables de Configuración (Seleccionadas en la UI) ---
        private int _currentEscenarioId = 0;
        private int _currentSensorId = 0; // <--- NUEVO: ID del sensor físico seleccionado

        public event Action<LecturaArduinoDto>? OnDataReceived;

        public SensorDataService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        // Método para recibir el Escenario desde Monitoreo.razor
        public void SetCurrentEscenario(int escenarioId)
        {
            _currentEscenarioId = escenarioId;
            Debug.WriteLine($"[SensorService] Escenario ID: {_currentEscenarioId}");
        }

        // --- NUEVO MÉTODO: Configurar Sensor Físico ---
        public void SetCurrentSensor(int sensorId)
        {
            _currentSensorId = sensorId;
            Debug.WriteLine($"[SensorService] Sensor ID: {_currentSensorId}");
        }

        public string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        public bool StartListening(string portName)
        {
            try
            {
                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen) _serialPort.Close();
                    _serialPort.DataReceived -= SerialPort_DataReceived;
                    _serialPort.Dispose();
                }

                _serialPort = new SerialPort(portName, 9600);
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
                Debug.WriteLine($"Serial port {portName} opened.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening serial port: {ex.Message}");
                return false;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                _jsonBuffer += sp.ReadExisting();

                string? line;
                while ((line = ReadLineFromBuffer()) != null)
                {
                    ProcessJsonLine(line);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private string? ReadLineFromBuffer()
        {
            int newlineIndex = _jsonBuffer.IndexOf('\n');
            if (newlineIndex == -1) return null;

            string line = _jsonBuffer.Substring(0, newlineIndex).Trim();
            _jsonBuffer = _jsonBuffer.Substring(newlineIndex + 1);
            return line;
        }

        private void ProcessJsonLine(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString) || !jsonString.StartsWith("{")) return;

            try
            {
                LecturaArduinoDto? lectura = JsonSerializer.Deserialize<LecturaArduinoDto>(jsonString);

                if (lectura != null)
                {
                    // 1. Notificar a la UI
                    OnDataReceived?.Invoke(lectura);

                    // 2. Guardar en BD (Solo si tenemos Escenario Y Sensor seleccionados)
                    if (_currentEscenarioId > 0 && _currentSensorId > 0)
                    {
                        Task.Run(() => SaveDataToDatabase(lectura));
                    }
                }
            }
            catch (JsonException ex) { Debug.WriteLine($"Error JSON: {ex.Message}"); }
        }

        private async Task SaveDataToDatabase(LecturaArduinoDto lectura)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // --- CLAVE: Buscar parámetros que pertenezcan al sensor seleccionado ---
                var paramsDict = await dbContext.Parametros
                    .Where(p => p.sensor_id == _currentSensorId)
                    .ToDictionaryAsync(p => p.nombre_parametro, p => p.parametro_id);

                if (!paramsDict.Any())
                {
                    Debug.WriteLine($"[Advertencia] El Sensor ID {_currentSensorId} no tiene parámetros configurados en la BD.");
                    return;
                }

                var timestamp = DateTime.UtcNow;
                int idUsuarioEjemplo = 1;
                var mediciones = new List<Medicion>();

                // Función local para mapear datos
                void AgregarMedicion(string nombreParametro, float valor, float cv, string? status)
                {
                    // Buscar ID del parámetro (ignora mayúsculas/minúsculas)
                    var key = paramsDict.Keys.FirstOrDefault(k => k.Equals(nombreParametro, StringComparison.OrdinalIgnoreCase));

                    if (key != null)
                    {
                        mediciones.Add(new Medicion
                        {
                            escenario_id = _currentEscenarioId,
                            sensor_id = _currentSensorId,       // <--- ID del Sensor REAL
                            parametro_id = paramsDict[key],     // <--- ID del Parámetro REAL
                            valor_analogico = (double)valor,
                            valor_cv_decimal = (decimal)cv,
                            status = (int)MapStatus(status),
                            fecha_hora = timestamp,
                            created_at = timestamp,
                            updated_at = timestamp,
                            user_id = idUsuarioEjemplo,
                            created_by = idUsuarioEjemplo
                        });
                    }
                }

                // Mapeo exacto a tu JSON
                AgregarMedicion("pH", lectura.PH, lectura.PHCV, lectura.PHSTATUS);
                AgregarMedicion("Turbidez", lectura.TURBIDEZNTU, lectura.TURBIDEZCV, lectura.TURBIDEZSTATUS);
                AgregarMedicion("Temperatura", lectura.TEMPERATURAC, lectura.TEMPERATURACV, "ideal");
                AgregarMedicion("Conductividad", lectura.CONDUCTIVIDADUSSCM, lectura.CONDUCTIVIDADCV, "ideal");

                if (mediciones.Any())
                {
                    await dbContext.Mediciones.AddRangeAsync(mediciones);
                    await dbContext.SaveChangesAsync();
                    Debug.WriteLine($"[DB] Guardadas {mediciones.Count} mediciones.");
                }
            }
        }

        private StatusMedicion MapStatus(string? status)
        {
            return status?.ToLower() switch
            {
                "ideal" => StatusMedicion.Ideal,
                "apta" => StatusMedicion.Apta,
                "cumple" => StatusMedicion.Apta,
                "no apta" => StatusMedicion.NoApta,
                "no_apto" => StatusMedicion.NoApta,
                "fuera_norma" => StatusMedicion.NoApta,
                _ => StatusMedicion.Ideal
            };
        }

        public void StopListening()
        {
            try
            {
                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen) _serialPort.Close();
                    _serialPort.DataReceived -= SerialPort_DataReceived;
                    _serialPort.Dispose();
                    _serialPort = null;
                }
            }
            catch { }
        }

        public void Dispose() => StopListening();
    }
}