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

        // --- NUEVO: Variable para guardar el Escenario seleccionado en la UI ---
        private int _currentEscenarioId = 0;

        public event Action<LecturaArduinoDto>? OnDataReceived;

        public SensorDataService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        // --- NUEVO: Método para recibir el ID desde la página Monitoreo.razor ---
        public void SetCurrentEscenario(int escenarioId)
        {
            _currentEscenarioId = escenarioId;
            Debug.WriteLine($"Servicio configurado para Escenario ID: {_currentEscenarioId}");
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
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                    }
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Error reading serial data: {ex.Message}");
            }
        }

        private string? ReadLineFromBuffer()
        {
            int newlineIndex = _jsonBuffer.IndexOf('\n');
            if (newlineIndex == -1)
            {
                return null;
            }

            string line = _jsonBuffer.Substring(0, newlineIndex).Trim();
            _jsonBuffer = _jsonBuffer.Substring(newlineIndex + 1);
            return line;
        }

        private void ProcessJsonLine(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString) || !jsonString.StartsWith("{") || !jsonString.EndsWith("}"))
            {
                Debug.WriteLine($"Data skipped (not JSON): {jsonString}");
                return;
            }

            try
            {
                Debug.WriteLine($"Data Received: {jsonString}");
                LecturaArduinoDto? lectura = JsonSerializer.Deserialize<LecturaArduinoDto>(jsonString);

                if (lectura != null)
                {
                    // 1. Notificar a la UI
                    OnDataReceived?.Invoke(lectura);

                    // 2. Guardar en la DB Local (MySQL)
                    Task.Run(() => SaveDataToDatabase(lectura));
                }
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"Error parsing JSON: {ex.Message} | JSON: {jsonString}");
            }
        }

        private async Task SaveDataToDatabase(LecturaArduinoDto lectura)
        {
            // Validar que se haya seleccionado un escenario
            if (_currentEscenarioId == 0)
            {
                Debug.WriteLine("ADVERTENCIA: No se guardaron datos porque no hay un escenario seleccionado.");
                return;
            }

            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Carga los parámetros (Asegúrate que los nombres coinciden con tu DB)
                var parametros = await dbContext.Parametros
                    .ToDictionaryAsync(p => p.nombre_parametro, p => p.parametro_id);

                var timestamp = DateTime.UtcNow; // Recomendado usar UTC
                int idSensorEjemplo = 1; // Esto podrías hacerlo dinámico también si tienes múltiples sensores físicos
                int idUsuarioEjemplo = 1; // O el ID del usuario logueado si tienes autenticación

                var mediciones = new List<Medicion>();

                // --- Guardar pH ---
                if (parametros.TryGetValue("pH", out int idPH))
                {
                    mediciones.Add(new Medicion
                    {
                        sensor_id = idSensorEjemplo,
                        parametro_id = idPH,
                        escenario_id = _currentEscenarioId, // USANDO LA VARIABLE DINÁMICA
                        valor_analogico = (double)lectura.PH,
                        status = (int)MapStatus(lectura.PhStatus),
                        valor_cv_decimal = (decimal)lectura.PhCV,
                        fecha_hora = timestamp,
                        created_at = timestamp,
                        user_id = idUsuarioEjemplo,
                        created_by = idUsuarioEjemplo
                    });
                }

                // --- Guardar Turbidez ---
                if (parametros.TryGetValue("Turbidez", out int idTurbidez))
                {
                    mediciones.Add(new Medicion
                    {
                        sensor_id = idSensorEjemplo,
                        parametro_id = idTurbidez,
                        escenario_id = _currentEscenarioId, // USANDO LA VARIABLE DINÁMICA
                        valor_analogico = (double)lectura.TurbidezNTU,
                        status = (int)MapStatus(lectura.TurbidezStatus),
                        valor_cv_decimal = (decimal)lectura.TurbidezCV,
                        fecha_hora = timestamp,
                        created_at = timestamp,
                        user_id = idUsuarioEjemplo,
                        created_by = idUsuarioEjemplo
                    });
                }

                // --- Guardar Temperatura ---
                if (parametros.TryGetValue("Temperatura", out int idTemp))
                {
                    mediciones.Add(new Medicion
                    {
                        sensor_id = idSensorEjemplo,
                        parametro_id = idTemp,
                        escenario_id = _currentEscenarioId, // USANDO LA VARIABLE DINÁMICA
                        valor_analogico = (double)lectura.TemperaturaC,
                        status = (int)StatusMedicion.Ideal,
                        valor_cv_decimal = (decimal)lectura.TemperaturaCV,
                        fecha_hora = timestamp,
                        created_at = timestamp,
                        user_id = idUsuarioEjemplo,
                        created_by = idUsuarioEjemplo
                    });
                }

                // --- Guardar Conductividad ---
                if (parametros.TryGetValue("Conductividad", out int idCond))
                {
                    mediciones.Add(new Medicion
                    {
                        sensor_id = idSensorEjemplo,
                        parametro_id = idCond,
                        escenario_id = _currentEscenarioId, // USANDO LA VARIABLE DINÁMICA
                        valor_analogico = (double)lectura.ConductividadUsScm,
                        status = (int)StatusMedicion.Ideal,
                        valor_cv_decimal = (decimal)lectura.ConductividadCV,
                        fecha_hora = timestamp,
                        created_at = timestamp,
                        user_id = idUsuarioEjemplo,
                        created_by = idUsuarioEjemplo
                    });
                }

                if (mediciones.Any())
                {
                    await dbContext.Mediciones.AddRangeAsync(mediciones);
                    await dbContext.SaveChangesAsync();
                    Debug.WriteLine($"Guardadas {mediciones.Count} mediciones en Escenario {_currentEscenarioId}");
                }
            }
        }

        private StatusMedicion MapStatus(string? status)
        {
            return status?.ToLower() switch
            {
                "ideal" => StatusMedicion.Ideal,
                "apta" => StatusMedicion.Apta,
                "no apta" => StatusMedicion.NoApta,
                _ => StatusMedicion.NoApta
            };
        }

        public void StopListening()
        {
            try
            {
                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                    }
                    _serialPort.DataReceived -= SerialPort_DataReceived;
                    _serialPort.Dispose();
                    _serialPort = null;
                    Debug.WriteLine("Serial port stopped and disposed.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error stopping serial port: {ex.Message}");
            }
        }

        public void Dispose()
        {
            StopListening();
        }
    }
}