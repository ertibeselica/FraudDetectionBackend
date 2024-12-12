using FraudDetection.Data.Transaction;
using FraudDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> GetTransactionById(int transactiondId)
        {
            var transaction = await _transactionRepository.GetById(transactiondId);
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            var transactions = await _transactionRepository.GetAll();
            return transactions;
        }
        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            var createdTrx = await _transactionRepository.Create(transaction);
            return createdTrx;
        }

        public async Task<bool> DeleteTransaction(int transactiondId)
        {
            var result = await _transactionRepository.Delete(transactiondId);
            return result;
        }        

        public async Task<Transaction> UpdateTransaction(Transaction transaction)
        {
            var updatedTrx = await _transactionRepository.Update(transaction);
            return updatedTrx;
        }

        public async Task<IEnumerable<Transaction>> GetSimilarTransactions(int transactionId)
        {
            var transaction = await GetTransactionById(transactionId);
            if (transaction == null)
                throw new KeyNotFoundException($"Transaction with ID {transactionId} not found.");

            var allTransactions = await GetTransactions();

            return allTransactions.Where(t =>
                (t.Id != transaction.Id) &&
                (t.Location == transaction.Location || t.Device == transaction.Device) &&
                (t.Amount >= transaction.Amount * (decimal)0.8 && t.Amount <= transaction.Amount * (decimal)1.2) &&
                (t.Time >= transaction.Time.AddDays(-30))
            )
            .OrderByDescending(t => t.Time)
            .Take(10);
        }
    }
}
