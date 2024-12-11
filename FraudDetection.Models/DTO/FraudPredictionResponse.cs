using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FraudDetection.Models.DTO
{
    public class FraudPredictionResponse
    {
        [JsonPropertyName("is_fraud")]
        public bool IsFraud { get; set; }

        [JsonPropertyName("anomaly_score")]
        public double AnomalyScore { get; set; }
    }
}
