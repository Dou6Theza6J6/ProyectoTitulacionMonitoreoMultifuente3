// MonitoreoMultifuente3/Services/SensorDataService.cs

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
using Microsoft.EntityFrameworkCore; // <-- Asegúrate de tener este
using System.Linq; // <-- Asegúrate de tener este

// --- 1. AÑADE ESTOS 2 USINGS PARA FIREBASE ---
using Firebase.Database;
using Firebase.Database.Query;

namespace MonitoreoMultifuente3.Services
{
    public class SensorDataService : IDisposable
    {
        private SerialPort? _serialPort;
        private readonly IServiceScopeFactory _scopeFactory;
        private string _jsonBuffer = string.Empty;

        // --- 2. AÑADE ESTAS 2 LÍNEAS PARA FIREBASE ---
        private readonly FirebaseClient _firebaseClient;
        // --- ¡¡CAMBIA ESTA URL POR LA DE TU PROYECTO!! ---
        private const string FirebaseDatabaseUrl = "https://console.firebase.google.com/u/6/project/monitoreoturbideznube/database/monitoreoturbideznube-default-rtdb/data/~2F?hl=es-419";

        public event Action<LecturaArduinoDto>? OnDataReceived;

        public SensorDataService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            // --- 3. AÑADE ESTA LÍNEA (INICIALIZA FIREBASE) ---
            _firebaseClient = new FirebaseClient(FirebaseDatabaseUrl);
        }

        // ... (GetAvailablePorts y StartListening se quedan igual) ...

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

        // --- 4. MODIFICA ESTE MÉTODO ---
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
                    // 1. Notificar a la UI (Sin cambios)
                    OnDataReceived?.Invoke(lectura);

                    // 2. Guardar en la DB Local (MySQL) (Sin cambios)
                    Task.Run(() => SaveDataToDatabase(lectura));

                    // --- 5. AÑADE ESTA LÍNEA: Guardar en Firebase ---
                    Task.Run(() => SaveDataToFirebase(lectura));
                }
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"Error parsing JSON: {ex.Message} | JSON: {jsonString}");
            }
        }

        // --- 6. AÑADE ESTE MÉTODO NUEVO COMPLETO ---
        private async Task SaveDataToFirebase(LecturaArduinoDto lectura)
        {
            try
            {
                // Guarda la lectura completa en un nodo "mediciones_en_vivo"
                // y usa el timestamp como clave única.
                await _firebaseClient
                  .Child("mediciones_en_vivo")
                  .Child(lectura.TimestampMs.ToString()) // ID único
                  .PutAsync(lectura);

                Debug.WriteLine($"Datos guardados en Firebase: {lectura.TimestampMs}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al guardar en Firebase: {ex.Message}");
            }
        }

        // --- 7. ESTE ES TU MÉTODO DE GUARDADO EN MYSQL (LO DEJAMOS IGUAL) ---
        // (Este es el código robusto que arregla el guardado de Turbidez, etc.)
        // MonitoreoMultifuente3/Services/SensorDataService.cs
        // (Solo reemplaza este método)
        // MonitoreoMultifuente3/Services/SensorDataService.cs
        // (Solo reemplaza este método)
        private async Task SaveDataToDatabase(LecturaArduinoDto lectura)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // --- CORRECCIÓN ---
                // Usamos 'nombre_parametro_varChar' para que coincida con tu modelo Parametro.cs
                var parametros = await dbContext.Parametros
                    .ToDictionaryAsync(p => p.nombre_parametro, p => p.parametro_id);

                var timestamp = DateTime.Now;
                int idSensorEjemplo = 1;
                int idEscenarioEjemplo = 1;
                int idUsuarioEjemplo = 1;

                var mediciones = new List<Medicion>();

                // ¡¡IMPORTANTE!!
                // Revisa que estos nombres ("pH", "Turbidez", etc.) sean
                // EXACTAMENTE iguales a como están en tu tabla 'parametros'.

                // Intenta guardar pH
                if (parametros.TryGetValue("pH", out int idPH))
                {
                    mediciones.Add(new Medicion
                    {
                        sensor_id = idSensorEjemplo,
                        parametro_id = idPH,
                        escenario_id = idEscenarioEjemplo,
                       valor_analogico = (double)lectura.PH,
                        status = (int)MapStatus(lectura.PhStatus),
                        valor_cv_decimal = (decimal)lectura.PhCV,
                       fecha_hora = timestamp, 
                        created_at = timestamp,
                        user_id= idUsuarioEjemplo,
                        created_by = idUsuarioEjemplo
                    });
                }
                else { Debug.WriteLine("Error de guardado: No se encontró el parámetro 'pH' en la BD."); }

                // Intenta guardar Turbidez
                if (parametros.TryGetValue("Turbidez", out int idTurbidez)) // <-- REVISA ESTE NOMBRE
                {
                    mediciones.Add(new Medicion
                    {
                        sensor_id = idSensorEjemplo,
                        parametro_id = idTurbidez,
                        escenario_id = idEscenarioEjemplo,
                      valor_analogico = (double)lectura.TurbidezNTU, 
                        status = (int)MapStatus(lectura.TurbidezStatus),
                        valor_cv_decimal = (decimal)lectura.TurbidezCV,
                       fecha_hora= timestamp,
                        created_at = timestamp,
                        user_id = idUsuarioEjemplo,
                        created_by= idUsuarioEjemplo
                    });
                }
                else { Debug.WriteLine("Error de guardado: No se encontró el parámetro 'Turbidez' en la BD."); }

                // Intenta guardar Temperatura
                if (parametros.TryGetValue("Temperatura", out int idTemp)) // <-- REVISA ESTE NOMBRE
                {
                    mediciones.Add(new Medicion
                    {
                        sensor_id = idSensorEjemplo,
                        parametro_id = idTemp,
                        escenario_id = idEscenarioEjemplo,
                        valor_analogico = (double)lectura.TemperaturaC,
                        status = (int)StatusMedicion.Ideal,
                        valor_cv_decimal = (decimal)lectura.TemperaturaCV,
                        fecha_hora = timestamp,
                        created_at = timestamp,
                        user_id = idUsuarioEjemplo,
                        created_by = idUsuarioEjemplo
                    });
                }
                else { Debug.WriteLine("Error de guardado: No se encontró el parámetro 'Temperatura' en la BD."); }

                // Intenta guardar Conductividad
                if (parametros.TryGetValue("Conductividad", out int idCond)) // <-- REVISA ESTE NOMBRE
                {
                    mediciones.Add(new Medicion
                    {
                        sensor_id = idSensorEjemplo,
                        parametro_id = idCond,
                        escenario_id = idEscenarioEjemplo,
                       valor_analogico = (double)lectura.ConductividadUsScm,
                        status = (int)StatusMedicion.Ideal,
                        valor_cv_decimal = (decimal)lectura.ConductividadCV,
                       fecha_hora = timestamp,
                        created_at = timestamp,
                        user_id = idUsuarioEjemplo,
                        created_by = idUsuarioEjemplo
                    });
                }
                else { Debug.WriteLine("Error de guardado: No se encontró el parámetro 'Conductividad' en la BD."); }

                if (mediciones.Any())
                {
                    await dbContext.Mediciones.AddRangeAsync(mediciones);
                    await dbContext.SaveChangesAsync();
                    Debug.WriteLine($"Guardadas {mediciones.Count} mediciones en la BD para el timestamp {timestamp}");
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

        // ... (StopListening y Dispose se quedan igual) ...
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