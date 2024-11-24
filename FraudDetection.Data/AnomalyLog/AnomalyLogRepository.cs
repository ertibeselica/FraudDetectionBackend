using FraudDetection.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Data.AnomalyLog
{
    public class AnomalyLogRepository : IAnomalyLogRepository
    {
        protected readonly FraudDetectionDbContext _context;

        public AnomalyLogRepository(FraudDetectionDbContext context)
        {
           _context = context;
        }
        public async Task<Models.AnomalyLog> Create(Models.AnomalyLog anomalyLog)
        {
            await _context.AnomalyLogs.AddAsync(anomalyLog);
            await _context.SaveChangesAsync();
            return anomalyLog;
        }

        public async Task<bool> Delete(int anomalyLogId)
        {
            var anomaly = await _context.AnomalyLogs.FindAsync(anomalyLogId);
            if (anomaly != null)
            {
                _context.AnomalyLogs.Remove(anomaly);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Models.AnomalyLog>> GetAll()
        {
            return await _context.AnomalyLogs.AsNoTracking().ToListAsync();
        }

        public async Task<Models.AnomalyLog> GetById(int anomalyLogId, bool includeRelations = false)
        {
            return await _context.AnomalyLogs.FirstOrDefaultAsync(anomaly => anomaly.Id == anomalyLogId);
        }

        public async Task<Models.AnomalyLog> Update(Models.AnomalyLog anomalyLog)
        {
            _context.AnomalyLogs.Update(anomalyLog);
            await _context.SaveChangesAsync();

            return anomalyLog;
        }
    }
}
