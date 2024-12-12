using AutoMapper;
using FraudDetection.Data.Entity;
using FraudDetection.Models;
using FraudDetection.Models.DTO;
using FraudDetection.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<TransactionHub> _hubContext;

        public TransactionsController(ITransactionService transactionService, IAnomalyLogService anomalyLogService, IFraudDetectionService fraudDetectionService, IMapper mapper, IHubContext<TransactionHub> hubContext)
        {
            _transactionService = transactionService;
            _anomalyLogService = anomalyLogService;
            _fraudDetectionService = fraudDetectionService;
            _mapper = mapper;
            _hubContext = hubContext;
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

                await _hubContext.Clients.All.SendAsync("ReceiveTransaction", createdTrx);

                // If fraudulent, send fraud alert
                if (isFraud)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveFraudAlert", createdTrx);
                }

                return Ok(new { transactionDto, anomalyLogDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the transaction.", details = ex.Message });
            }
        }

        [HttpGet("similar/{id}")]
        public async Task<IActionResult> GetSimilarTransactions(int id)
        {
            try
            {
                var similarTransactions = await _transactionService.GetSimilarTransactions(id);
                return Ok(similarTransactions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching similar transactions.", details = ex.Message });
            }
        }

        [HttpPost("batch")]
        public async Task<IActionResult> ProcessBatchTransactions([FromBody] BatchTransactionRequest request)
        {
            try
            {
                var results = new List<(bool Success, bool IsFraud)>();

                foreach (var transaction in request.Transactions)
                {
                    try
                    {
                        var (isFraud, score) = await _fraudDetectionService.PredictFraudAsync(transaction);
                        var trx = _mapper.Map<Models.Transaction>(transaction);
                        trx.IsFraud = isFraud;
                        await _transactionService.CreateTransaction(trx);

                        var anomalyLog = new AnomalyLog
                        {
                            TransactionId = trx.Id,
                            Score = score,
                            Decision = isFraud,
                            CreatedAt = DateTime.UtcNow
                        };

                        var createdAnomalyLog = await _anomalyLogService.CreateAnomaly(anomalyLog);


                        // Send real-time update
                        await _hubContext.Clients.All.SendAsync("ReceiveTransaction", trx);
                        if (isFraud)
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveFraudAlert", trx);
                        }

                        results.Add((true, isFraud));
                    }
                    catch
                    {
                        results.Add((false, false));
                    }
                }

                return Ok(new
                {
                    Successful = results.Count(r => r.Success),
                    Failed = results.Count(r => !r.Success),
                    Fraudulent = results.Count(r => r.Success && r.IsFraud)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to process batch", details = ex.Message });
            }
        }
    }
}

