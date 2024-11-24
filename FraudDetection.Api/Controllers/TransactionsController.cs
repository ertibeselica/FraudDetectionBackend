using FraudDetection.Data.Entity;
using FraudDetection.Models;
using FraudDetection.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FraudDetection.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IAnomalyLogService _anomalyLogService;
        private readonly IFraudDetectionService _fraudDetectionService;

        public TransactionsController(ITransactionService transactionService,IAnomalyLogService anomalyLogService, IFraudDetectionService fraudDetectionService)
        {
            _transactionService = transactionService;
            _anomalyLogService = anomalyLogService;
            _fraudDetectionService = fraudDetectionService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService.GetTransactions();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _transactionService.GetTransactionById(id);

            if (transaction == null)
                return NotFound("Transaction not found.");

            return Ok(transaction);
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null) 
                return BadRequest("Transaction data is required.");

            try
            {
                var (isFraud, score) = await _fraudDetectionService.PredictFraudAsync(transaction);

                transaction.IsFraud = isFraud;

                var createdTrx =  await _transactionService.CreateTransaction(transaction);

                var anomalyLog = new AnomalyLog
                {
                    TransactionId = transaction.Id,
                    Score = score,
                    Decision = isFraud,
                    CreatedAt = DateTime.UtcNow
                };

                var createdAnomalyLog = _anomalyLogService.CreateAnomaly(anomalyLog);

                return Ok(new { createdTrx, createdAnomalyLog });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the transaction.", details = ex.Message });
            }
        }        
    }
}

