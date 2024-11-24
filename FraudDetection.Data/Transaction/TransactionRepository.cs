using FraudDetection.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Data.Transaction
{
    public class TransactionRepository : ITransactionRepository
    {
        protected readonly FraudDetectionDbContext _context;

        public TransactionRepository(FraudDetectionDbContext context)
        {
          _context = context;
        }
        public async Task<Models.Transaction> Create(Models.Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> Delete(int transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Models.Transaction>> GetAll()
        {
            return await _context.Transactions.AsNoTracking().ToListAsync();
        }

        public async Task<Models.Transaction> GetById(int transactionId, bool includeRelations = false)
        {
            return await _context.Transactions.FirstOrDefaultAsync(trx => trx.Id == transactionId);
        }

        public async Task<Models.Transaction> Update(Models.Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }
    }
}
