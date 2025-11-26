using System;
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

        private int _currentEscenarioId = 0;
        private int _currentSensorId = 0;   // 0 = TODOS LOS SENSORES
        private int _currentUserId = 0;

        public event Action<LecturaArduinoDto>? OnDataReceived;
        public event Action<string>? OnLog;

        public bool IsConnected => _serialPort != null && _serialPort.IsOpen;
        public string ConnectedPortName => _serialPort?.PortName ?? "";

        public SensorDataService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void SetCurrentEscenario(int id) => _currentEscenarioId = id;
        public void SetCurrentSensor(int id) => _currentSensorId = id; // 0 = TODOS
        public void SetCurrentUser(int id) => _currentUserId = id;

        public string[] GetAvailablePorts() => SerialPort.GetPortNames();

        public async Task<bool> AutoConectar()
        {
            StopListening();
            var puertos = SerialPort.GetPortNames();

            if (puertos.Length == 0)
            {
                OnLog?.Invoke("❌ Windows no detecta puertos. Conecta el USB.");
                return false;
            }

            OnLog?.Invoke($"🔎 Escaneando {puertos.Length} puertos...");

            foreach (var puerto in puertos)
            {
                if ((puerto == "COM1" || puerto == "COM2") && puertos.Length > 1) continue;

                try
                {
                    OnLog?.Invoke($"👉 Analizando {puerto} (Esperando 13 seg)...");

                    using (var puertoPrueba = new SerialPort(puerto, 9600))
                    {
                        puertoPrueba.ReadTimeout = 14000;
                        puertoPrueba.DtrEnable = true;
                        puertoPrueba.RtsEnable = true;
                        puertoPrueba.Open();

                        await Task.Delay(13000);

                        if (puertoPrueba.BytesToRead > 0)
                        {
                            string datos = puertoPrueba.ReadExisting();
                            OnLog?.Invoke($"✅ ¡DATOS RECIBIDOS! -> {datos.Substring(0, Math.Min(datos.Length, 50))}...");
                            puertoPrueba.Close();
                            return StartListening(puerto);
                        }
                        else
                        {
                            OnLog?.Invoke($"⚠️ {puerto} sin respuesta tras 13 seg.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnLog?.Invoke($"❌ {puerto}: {ex.Message}");
                }
            }

            OnLog?.Invoke("🏁 Fin del escaneo. Ningún sensor habló en el tiempo esperado.");
            return false;
        }

        public bool StartListening(string portName)
        {
            try
            {
                StopListening();
                _serialPort = new SerialPort(portName, 9600);
                _serialPort.DtrEnable = true;
                _serialPort.RtsEnable = true;
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
                OnLog?.Invoke($"🚀 CONECTADO A {portName}. ESPERA A QUE LLEGUEN DATOS.");
                return true;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"❌ Error al conectar {portName}: {ex.Message}");
                return false;
            }
        }

        public void StopListening()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.DataReceived -= SerialPort_DataReceived;
                try { _serialPort.Close(); } catch { }
                _serialPort.Dispose();
                _serialPort = null;
                OnLog?.Invoke("🔌 Desconectado.");
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string data = sp.ReadExisting();
                _jsonBuffer += data;

                string? line;
                while ((line = ExtractLine()) != null)
                {
                    ProcessJson(line);
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"⚠️ Error lectura serial: {ex.Message}");
            }
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

            OnLog?.Invoke($"📥 JSON crudo: {json}");

            try
            {
                var data = JsonSerializer.Deserialize<LecturaArduinoDto>(json);
                if (data != null)
                {
                    OnLog?.Invoke(
                        $"📊 Decodificado: pH={data.PH:0.00}, NTU={data.Turbidez_NTU:0}, T={data.Temperatura_C:0.0}, EC={data.Conductividad_uScm:0.0}"
                    );

                    OnDataReceived?.Invoke(data);
                    Task.Run(() => ProcesarYGuardar(data));
                }
            }
            catch (JsonException)
            {
                OnLog?.Invoke("⚠️ JSON incompleto o inválido recibido.");
            }
        }

        private async Task ProcesarYGuardar(LecturaArduinoDto data)
        {
            if (_currentEscenarioId <= 0 || _currentUserId == 0)
            {
                OnLog?.Invoke("ℹ️ Datos recibidos pero NO guardados: falta Escenario o Usuario.");
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var fecha = DateTime.Now;

            // CASO 1: un solo sensor seleccionado
            if (_currentSensorId > 0)
            {
                var parametros = await db.Parametros
                    .Where(p => p.sensor_id == _currentSensorId)
                    .AsNoTracking()
                    .ToListAsync();

                if (!parametros.Any())
                {
                    OnLog?.Invoke($"ℹ️ El sensor {_currentSensorId} no tiene parámetros configurados. No se guarda nada.");
                    return;
                }

                foreach (var p in parametros)
                {
                    GuardarSegunParametro(db, p, data, fecha, _currentSensorId);
                }
            }
            // CASO 2: todos los sensores
            else
            {
                var sensoresConParametros = await db.Parametros
                    .AsNoTracking()
                    .GroupBy(p => p.sensor_id)
                    .ToListAsync();

                if (!sensoresConParametros.Any())
                {
                    OnLog?.Invoke("ℹ️ No hay sensores con parámetros configurados. No se guarda nada.");
                    return;
                }

                foreach (var grupo in sensoresConParametros)
                {
                    int sensorId = grupo.Key;
                    foreach (var p in grupo)
                    {
                        GuardarSegunParametro(db, p, data, fecha, sensorId);
                    }
                }
            }

            if (db.ChangeTracker.HasChanges())
            {
                await db.SaveChangesAsync();
                OnLog?.Invoke("💾 Mediciones guardadas correctamente en la BD.");
            }
        }

        private void GuardarSegunParametro(ApplicationDbContext db, Parametro p, LecturaArduinoDto data, DateTime fecha, int sensorId)
        {
            if (IsParam(p, "pH"))
                Guardar(db, p.parametro_id, sensorId, data.PH, data.PH_CV, data.PH_Status, fecha);
            else if (IsParam(p, "Turbidez"))
                Guardar(db, p.parametro_id, sensorId, data.Turbidez_NTU, data.Turbidez_CV, data.Turbidez_Status, fecha);
            else if (IsParam(p, "Temperatura"))
                Guardar(db, p.parametro_id, sensorId, data.Temperatura_C, data.Temperatura_CV, "ideal", fecha);
            else if (IsParam(p, "Conductividad"))
                Guardar(db, p.parametro_id, sensorId, data.Conductividad_uScm, data.Conductividad_CV, "ideal", fecha);
        }

        private bool IsParam(Parametro p, string name)
            => p.nombre_parametro.Equals(name, StringComparison.OrdinalIgnoreCase);

        private void Guardar(ApplicationDbContext db, int paramId, int sensorId, float val, float cv, string status, DateTime fecha)
        {
            db.Mediciones.Add(new Medicion
            {
                escenario_id = _currentEscenarioId,
                sensor_id = sensorId,
                user_id = _currentUserId,
                created_by = _currentUserId,
                parametro_id = paramId,
                fecha_hora = fecha,
                created_at = fecha,
                updated_at = fecha,
                valor_analogico = (double)val,
                valor_cv_decimal = (decimal)cv,   // ← AQUÍ VA EL CV DEL JSON
                status = (int)MapStatus(status),
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

        public void Dispose() => StopListening();
    }
}
