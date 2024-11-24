using FraudDetection.Data.AnomalyLog;
using FraudDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Services
{
    public class AnomalyLogService : IAnomalyLogService
    {
        private readonly IAnomalyLogRepository _anomalyRepository;

        public AnomalyLogService(IAnomalyLogRepository anomalyRepository)
        {
                _anomalyRepository = anomalyRepository;
        }
        public async Task<AnomalyLog> CreateAnomaly(AnomalyLog anomaly)
        {
            var createdAnomaly = await _anomalyRepository.Create(anomaly);
            return createdAnomaly;
        }

        public async Task<bool> DeleteAnomaly(int anomalyId)
        {
            var result = await _anomalyRepository.Delete(anomalyId);
            return result;
        }

        public async Task<IEnumerable<AnomalyLog>> GetAnomalies()
        {
            var anomalies = await _anomalyRepository.GetAll();
            return anomalies;
        }

        public async Task<AnomalyLog> GetAnomalyById(int anomalyId)
        {
            var anomaly = await _anomalyRepository.GetById(anomalyId);
            return anomaly;
        }

        public async Task<AnomalyLog> UpdateAnomaly(AnomalyLog anomaly)
        {
            var updatedAnomaly = await _anomalyRepository.Update(anomaly);
            return updatedAnomaly;
        }
    }
}
