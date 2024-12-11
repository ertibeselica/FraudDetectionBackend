using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Services
{
    public interface IFraudDetectionService
    {
        Task<(bool isFraud, double score)> PredictFraudAsync(Models.DTO.TransactionDto transaction);
    }
}
