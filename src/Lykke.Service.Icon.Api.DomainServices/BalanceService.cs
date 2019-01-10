using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Icon.Api.Core.Domain;
using Lykke.Service.Icon.Api.Core.Repositories;
using Lykke.Service.Icon.Api.Core.Services;

namespace Lykke.Service.Icon.Api.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;

        public BalanceService(IBalanceRepository balanceRepository)
        {
            _balanceRepository = balanceRepository;
        }

        public async Task<bool> BeginObservationIfNotObservingAsync(string address)
        {
            var result = await _balanceRepository.CreateIfNotExistsAsync(address);

            return result;
        }

        public async Task<bool> EndObservationIfObservingAsync(string address)
        {
            var result = await _balanceRepository.DeleteIfExistsAsync(address);

            return result;
        }

        public async Task<(IEnumerable<Balance> balances, string continuationToken)> GetTransferableBalancesAsync(int take, string continuationToken)
        {
             var (balances, cToken) = await _balanceRepository.GetAllTransferableBalancesAsync(take, continuationToken);

            return (balances, cToken);
        }
    }
}
