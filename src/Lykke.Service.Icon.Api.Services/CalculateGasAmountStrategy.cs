using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Service.Icon.Api.Services
{
    public class CalculateGasAmountStrategy : ICalculateGasAmountStrategy
    {
        public Task<GasAmountCalculationResult> ExecuteAsync(IAddressService addressService, IBlockchainService blockchainService, string @from, string to,
            BigInteger transferAmount)
        {
            return Task.FromResult((GasAmountCalculationResult)GasAmountCalculationResult.GasAmount(100_000));
        }
    }
}
