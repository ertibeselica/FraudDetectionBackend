using FraudDetection.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Services
{
    public class FraudDetectionService : IFraudDetectionService
    {
        private readonly HttpClient _httpClient;

        public FraudDetectionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool isFraud, double score)> PredictFraudAsync(Models.DTO.TransactionDto transaction)
        {
            var response = await _httpClient.PostAsJsonAsync("http://127.0.0.1:5000/predict", transaction);
            var result = await response.Content.ReadFromJsonAsync<FraudPredictionResponse>();

            if (result != null)
            {
                bool isFraud = result.IsFraud;
                double score = result.AnomalyScore;

                return (isFraud, score);
            }

            throw new InvalidOperationException("The transaction fraud processing terminated unexpectedly");
        }
    }
}
