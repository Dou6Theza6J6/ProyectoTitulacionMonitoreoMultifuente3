using System;
using System.Collections.Concurrent;
using System.Linq;

namespace MonitoreoMultifuente3.Services
{
    public enum SensorState { Activo, Inactivo, Error }

    public class SensorStatusService
    {
        private readonly ConcurrentDictionary<int, DateTime> _lastSeen = new();

        public void UpdateSensorStatus(int sensorId)
        {
            _lastSeen[sensorId] = DateTime.UtcNow;
        }

        public SensorState GetSensorState(int sensorId)
        {
            if (_lastSeen.TryGetValue(sensorId, out var lastSeenTime))
            {
                // Si hemos recibido datos en los últimos 30 segundos, está activo.
                if (DateTime.UtcNow - lastSeenTime < TimeSpan.FromSeconds(30))
                {
                    return SensorState.Activo;
                }
            }
            // Si no, se considera inactivo.
            return SensorState.Inactivo;
        }
    }
}
