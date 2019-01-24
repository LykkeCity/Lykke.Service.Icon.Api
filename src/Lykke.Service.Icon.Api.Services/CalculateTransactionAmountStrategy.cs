using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Domain.Services.Strategies;
using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Service.Icon.Api.Services
{
    public class CalculateTransactionAmountStrategy : ICalculateTransactionAmountStrategy
    {
        private static BigInteger _transactionCost = BigInteger.Parse("1000000000000000");

        public Task<TransactionAmountCalculationResult> ExecuteAsync(IBlockchainService blockchainService, string @from, BigInteger transferAmount, BigInteger gasAmount,
            bool includeFee)
        {
            BigInteger result = transferAmount;
            if (includeFee)
                result -= _transactionCost;

            if (result <= 0)
                return Task.FromResult((TransactionAmountCalculationResult)TransactionAmountCalculationResult.TransactionAmountIsTooSmall());

            return Task.FromResult((TransactionAmountCalculationResult)TransactionAmountCalculationResult.TransactionAmount(result));
        }
    }
}
