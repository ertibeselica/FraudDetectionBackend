using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Services
{
    public interface IAnomalyLogService
    {
        Task<Models.AnomalyLog> GetAnomalyById(int anomalyId);
        Task<IEnumerable<Models.AnomalyLog>> GetAnomalies();
        Task<Models.AnomalyLog> CreateAnomaly(Models.AnomalyLog anomaly);
        Task<Models.AnomalyLog> UpdateAnomaly(Models.AnomalyLog anomaly);
        Task<bool> DeleteAnomaly(int anomalyId);
    }
}
