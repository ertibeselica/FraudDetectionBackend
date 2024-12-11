using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Models.DTO
{
    public class AnomalyLogResponseDto
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public double Score { get; set; }
        public bool Decision { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
