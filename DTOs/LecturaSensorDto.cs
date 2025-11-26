using System.Text.Json.Serialization;

namespace MonitoreoMultifuente3.DTOs
{
    public class LecturaArduinoDto
    {
        // --- pH ---
        [JsonPropertyName("pH")]
        public float PH { get; set; }

        [JsonPropertyName("pH_CV")]
        public float PhCV { get; set; }

        [JsonPropertyName("pH_status")]
        public string? PhStatus { get; set; }

        // --- Turbidez ---
        [JsonPropertyName("turbidez_NTU")]
        public float TurbidezNTU { get; set; }

        [JsonPropertyName("turbidez_CV")]
        public float TurbidezCV { get; set; }

        [JsonPropertyName("turbidez_status")]
        public string? TurbidezStatus { get; set; }

        // --- Temperatura ---
        [JsonPropertyName("temperatura_C")]
        public float TemperaturaC { get; set; }

        [JsonPropertyName("temperatura_CV")]
        public float TemperaturaCV { get; set; }

        // --- Conductividad ---
        [JsonPropertyName("conductividad_uScm")]
        public float ConductividadUsScm { get; set; }

        [JsonPropertyName("conductividad_CV")]
        public float ConductividadCV { get; set; }
    }
}