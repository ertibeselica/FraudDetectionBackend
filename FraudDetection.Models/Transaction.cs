using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FraudDetection.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
        public string Location { get; set; }
        public string Device { get; set; }
        public bool? IsFraud { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<AnomalyLog> AnomalyLogs { get; set; }
    }
}
