using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FraudDetection.Models.DTO
{
    public class TransactionDto
    {
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
        public string Location { get; set; }
        public string Device { get; set; }
    }
}
