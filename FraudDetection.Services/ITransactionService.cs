using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Services
{
    public interface ITransactionService
    {
        Task<Models.Transaction> GetTransactionById(int transactiondId);
        Task<IEnumerable<Models.Transaction>> GetTransactions();        
        Task<Models.Transaction> CreateTransaction(Models.Transaction transaction);
        Task<Models.Transaction> UpdateTransaction(Models.Transaction transaction);
        Task<bool> DeleteTransaction(int transactiondId);

    }
}
