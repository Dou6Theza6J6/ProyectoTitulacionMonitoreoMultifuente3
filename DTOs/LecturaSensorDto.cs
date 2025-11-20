// MonitoreoMultifuente3/DTOs/LecturaArduinoDto.cs
using System.Text.Json.Serialization;

namespace MonitoreoMultifuente3.DTOs
{
    public class LecturaArduinoDto
    {
        // Campos que vienen del JSON pero NO guardaremos en la lógica (Son INNECESARIOS)
        [JsonPropertyName("timestamp_ms")]
        public long TimestampMs { get; set; }

        [JsonPropertyName("escenario")]
        public string? EscenarioArduino { get; set; } // Lo ignoramos para guardar, usamos el seleccionado en UI

        // --- PARÁMETROS A GUARDAR ---

        // 1. pH
        [JsonPropertyName("pH")]
        public float PH { get; set; }

        [JsonPropertyName("pH_CV")]
        public float PhCV { get; set; }

        [JsonPropertyName("pH_status")]
        public string? PhStatus { get; set; }

        // 2. Turbidez
        [JsonPropertyName("turbidez_NTU")]
        public float TurbidezNTU { get; set; }

        [JsonPropertyName("turbidez_CV")]
        public float TurbidezCV { get; set; }

        [JsonPropertyName("turbidez_status")]
        public string? TurbidezStatus { get; set; }

        // 3. Temperatura
        [JsonPropertyName("temperatura_C")]
        public float TemperaturaC { get; set; }

        [JsonPropertyName("temperatura_CV")]
        public float TemperaturaCV { get; set; }

        // 4. Conductividad
        [JsonPropertyName("conductividad_uScm")]
        public float ConductividadUsScm { get; set; }

        [JsonPropertyName("conductividad_CV")]
        public float ConductividadCV { get; set; }
    }
}