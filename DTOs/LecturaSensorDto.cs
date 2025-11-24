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
        public float PHCV { get; set; }

        [JsonPropertyName("pH_status")]
        public string? PHSTATUS { get; set; }

        // 2. Turbidez
        [JsonPropertyName("turbidez_NTU")]
        public float TURBIDEZNTU { get; set; }

        [JsonPropertyName("turbidez_CV")]
        public float TURBIDEZCV { get; set; }

        [JsonPropertyName("turbidez_status")]
        public string? TURBIDEZSTATUS { get; set; }

        // 3. Temperatura
        [JsonPropertyName("temperatura_C")]
        public float TEMPERATURAC { get; set; }

        [JsonPropertyName("temperatura_CV")]
        public float TEMPERATURACV { get; set; }

        // 4. Conductividad
        [JsonPropertyName("conductividad_uScm")]
        public float CONDUCTIVIDADUSSCM { get; set; }

        [JsonPropertyName("conductividad_CV")]
        public float CONDUCTIVIDADCV { get; set; }
    }
}