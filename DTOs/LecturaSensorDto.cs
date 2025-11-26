using System.Text.Json.Serialization;

namespace MonitoreoMultifuente3.DTOs
{
    public class LecturaArduinoDto
    {
        [JsonPropertyName("pH")]
        public float PH { get; set; }

        [JsonPropertyName("pH_CV")]
        public float PH_CV { get; set; }

        [JsonPropertyName("pH_status")]
        public string PH_Status { get; set; } = "";

        [JsonPropertyName("turbidez_NTU")]
        public float Turbidez_NTU { get; set; }

        [JsonPropertyName("turbidez_CV")]
        public float Turbidez_CV { get; set; }

        [JsonPropertyName("turbidez_status")]
        public string Turbidez_Status { get; set; } = "";

        [JsonPropertyName("temperatura_C")]
        public float Temperatura_C { get; set; }

        [JsonPropertyName("temperatura_CV")]
        public float Temperatura_CV { get; set; }

        [JsonPropertyName("conductividad_uScm")]
        public float Conductividad_uScm { get; set; }

        [JsonPropertyName("conductividad_CV")]
        public float Conductividad_CV { get; set; }
    }
}
