using AutoMapper;
using FraudDetection.Data.Entity;
using FraudDetection.Models;
using FraudDetection.Models.DTO;
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
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionService transactionService,IAnomalyLogService anomalyLogService, IFraudDetectionService fraudDetectionService,IMapper mapper)
        {
            _transactionService = transactionService;
            _anomalyLogService = anomalyLogService;            
            _fraudDetectionService = fraudDetectionService;
            _mapper = mapper;
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
        public async Task<IActionResult> ProcessTransaction([FromBody] TransactionDto transaction)
        {
            if (transaction == null) 
                return BadRequest("Transaction data is required.");


            try
            {
                var (isFraud, score) = await _fraudDetectionService.PredictFraudAsync(transaction);

                var trx = _mapper.Map<Models.Transaction>(transaction);
                trx.IsFraud = isFraud;

                var createdTrx =  await _transactionService.CreateTransaction(trx);

                var anomalyLog = new AnomalyLog
                {
                    TransactionId = createdTrx.Id,
                    Score = score,
                    Decision = isFraud,
                    CreatedAt = DateTime.UtcNow
                };

                var createdAnomalyLog = await _anomalyLogService.CreateAnomaly(anomalyLog);

                var transactionDto = new TransactionResponseDto
                {
                    Id = createdTrx.Id,
                    Amount = createdTrx.Amount,   
                    IsFraud = createdTrx.IsFraud
                };

                var anomalyLogDto = new AnomalyLogResponseDto
                {
                    Id = createdAnomalyLog.Id,
                    TransactionId = createdAnomalyLog.TransactionId,
                    Score = createdAnomalyLog.Score,
                    Decision = createdAnomalyLog.Decision,
                    CreatedAt = createdAnomalyLog.CreatedAt
                };

                return Ok(new { transactionDto, anomalyLogDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the transaction.", details = ex.Message });
            }
        }        
    }
}

