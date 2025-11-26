using System.Text.Json.Serialization;

namespace MonitoreoMultifuente3.DTOs
{
    // Esta clase es el "espejo" exacto de tu JSON de Arduino
    public class LecturaArduinoDto
    {
        // Arduino: doc["pH"] = ph;
        [JsonPropertyName("pH")]
        public float PH { get; set; }

        // Arduino: doc["pH_CV"] = cv_ph;
        [JsonPropertyName("pH_CV")]
        public float PH_CV { get; set; }

        // Arduino: doc["pH_status"]
        [JsonPropertyName("pH_status")]
        public string PH_Status { get; set; } = "";

        // Arduino: doc["turbidez_NTU"]
        [JsonPropertyName("turbidez_NTU")]
        public float Turbidez_NTU { get; set; }

        // Arduino: doc["turbidez_CV"]
        [JsonPropertyName("turbidez_CV")]
        public float Turbidez_CV { get; set; }

        // Arduino: doc["turbidez_status"]
        [JsonPropertyName("turbidez_status")]
        public string Turbidez_Status { get; set; } = "";

        // Arduino: doc["temperatura_C"]
        [JsonPropertyName("temperatura_C")]
        public float Temperatura_C { get; set; }

        // Arduino: doc["temperatura_CV"]
        [JsonPropertyName("temperatura_CV")]
        public float Temperatura_CV { get; set; }

        // Arduino: doc["conductividad_uScm"]
        [JsonPropertyName("conductividad_uScm")]
        public float Conductividad_uScm { get; set; }

        // Arduino: doc["conductividad_CV"]
        [JsonPropertyName("conductividad_CV")]
        public float Conductividad_CV { get; set; }
    }
}