using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract.Transactions;
using Lykke.Service.Icon.Api.Core;
using Lykke.Service.Icon.Api.Core.Domain;
using Lykke.Service.Icon.Api.Core.Services;
using Lykke.Service.Icon.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.Icon.Api.Controllers
{
    [PublicAPI, Route("/api/transactions/history")]
    public class TransactionsHistoryController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;

        public TransactionsHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }
        
        
        [HttpGet("to/{address}")]
        public async Task<ActionResult<IEnumerable<HistoricalTransactionContract>>> GetIncomingHistory(
            TransactionHistoryRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            var transactions = await _transactionHistoryService.GetIncomingHistoryAsync
            (
                address,
                request.Take,
                request.AfterHash
            );
            
            return Ok(transactions.Select(MapTransactionReceipt));
        }

        [HttpGet("from/{address}")]
        public async Task<ActionResult<IEnumerable<HistoricalTransactionContract>>> GetOutgoingHistory(
            TransactionHistoryRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            var transactions = await _transactionHistoryService.GetOutgoingHistoryAsync
            (
                address,
                request.Take,
                request.AfterHash
            );
            
            return Ok(transactions.Select(MapTransactionReceipt));
        }

        private static HistoricalTransactionContract MapTransactionReceipt(
            TransactionReceipt transactionReceipt)
        {
            return new HistoricalTransactionContract
            {
                Amount = transactionReceipt.Amount.ToString(),
                AssetId = Constants.AssetId,
                FromAddress = transactionReceipt.From,
                Hash = transactionReceipt.Hash,
                Timestamp = DateTimeOffset.FromUnixTimeSeconds((long) transactionReceipt.Timestamp).UtcDateTime,
                ToAddress = transactionReceipt.To
            };
        }
    }
}
