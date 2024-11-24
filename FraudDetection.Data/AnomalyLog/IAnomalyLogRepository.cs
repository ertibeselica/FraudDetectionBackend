using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Data.AnomalyLog
{
    public interface IAnomalyLogRepository
    {
        Task<IEnumerable<Models.AnomalyLog>> GetAll();
        Task<Models.AnomalyLog> GetById(int anomalyLogId, bool includeRelations = false);
        Task<Models.AnomalyLog> Create(Models.AnomalyLog anomalyLog);
        Task<Models.AnomalyLog> Update(Models.AnomalyLog anomalyLog);
        Task<bool> Delete(int anomalyLogId);
    }
}
