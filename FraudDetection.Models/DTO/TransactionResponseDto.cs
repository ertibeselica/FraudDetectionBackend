using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Models.DTO
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public bool? IsFraud { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
