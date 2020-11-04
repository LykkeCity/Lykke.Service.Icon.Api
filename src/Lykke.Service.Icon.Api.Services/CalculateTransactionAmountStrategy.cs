using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Domain.Services.Strategies;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;

namespace Lykke.Service.Icon.Api.Services
{
    public class CalculateTransactionAmountStrategy : ICalculateTransactionAmountStrategy
    {
        private readonly IIconService _iconService;

        public CalculateTransactionAmountStrategy(IIconService iconService)
        {
            _iconService = iconService;
        }
        public async Task<TransactionAmountCalculationResult> ExecuteAsync(IBlockchainService blockchainService, string @from, BigInteger transferAmount, BigInteger gasAmount,
            bool includeFee)
        {
            var result = transferAmount;
            if (includeFee)
            {
                var call = new Call.Builder()
                    .To(new Address("cx0000000000000000000000000000000000000001"))
                    .Method("getStepPrice")
                    .BuildWith<BigInteger>();

                // ReSharper disable once UnusedVariable
                var gasStepPrice = await _iconService.CallAsync(call);
                var transactionCost = gasAmount * gasStepPrice;
                result -= transactionCost;
            }

            if (result <= 0)
                return TransactionAmountCalculationResult.TransactionAmountIsTooSmall();

            return TransactionAmountCalculationResult.TransactionAmount(result);
        }
    }
}
