using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FraudDetection.Models;
using Microsoft.EntityFrameworkCore;
namespace FraudDetection.Data.Entity
{
    public class FraudDetectionDbContext : DbContext
    {
        public FraudDetectionDbContext(DbContextOptions<FraudDetectionDbContext> options) : base(options)
        {            
        }

        public DbSet<Models.Transaction> Transactions { get; set; }
        public DbSet<AnomalyLog> AnomalyLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
