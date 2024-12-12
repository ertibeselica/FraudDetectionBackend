using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Models.DTO
{
    public class BatchTransactionRequest
    {
        public List<TransactionDto> Transactions { get; set; } = new();
    }
}
