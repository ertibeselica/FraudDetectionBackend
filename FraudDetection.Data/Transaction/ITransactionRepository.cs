using FraudDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Data.Transaction
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Models.Transaction>> GetAll();
        Task<Models.Transaction> GetById(int transactionId, bool includeRelations = false);
        Task<Models.Transaction> Create(Models.Transaction transaction);
        Task<Models.Transaction> Update(Models.Transaction transaction);
        Task<bool> Delete(int transactionId);
    }
}
