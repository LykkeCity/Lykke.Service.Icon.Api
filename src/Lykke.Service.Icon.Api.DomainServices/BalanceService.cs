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
        private readonly IBalanceObservationRepository _balanceObservationRepository;

        public BalanceService(IBalanceRepository balanceRepository,
            IBalanceObservationRepository balanceObservationRepository)
        {
            _balanceRepository = balanceRepository;
            _balanceObservationRepository = balanceObservationRepository;
        }

        public async Task<bool> BeginObservationIfNotObservingAsync(string address)
        {
            var result = await _balanceObservationRepository.BeginObservationIfNotObservingAsync(address);

            return result;
        }

        public async Task<bool> EndObservationIfObservingAsync(string address)
        {
            var result = await _balanceObservationRepository.EndObservationIfNotObservingAsync(address);

            return result;
        }

        public async Task<(IEnumerable<AddressBalance> balances, string continuationToken)> GetTransferableBalancesAsync(int take, string continuationToken)
        {
             var (balances, cToken) = await _balanceRepository.GetTransferableBalancesAsync(take, continuationToken);

            return (balances, cToken);
        }
    }
}
