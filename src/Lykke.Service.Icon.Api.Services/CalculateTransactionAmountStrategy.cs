using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Service.Icon.Api.Services
{
    public class CalculateTransactionAmountStrategy : ICalculateTransactionAmountStrategy
    {
        public Task<TransactionAmountCalculationResult> ExecuteAsync(IBlockchainService blockchainService, string @from, BigInteger transferAmount, BigInteger gasAmount,
            bool includeFee)
        {
            return Task.FromResult((TransactionAmountCalculationResult)TransactionAmountCalculationResult.TransactionAmount(100_000));
        }
    }
}
