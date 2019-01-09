using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Icon.Api.Core.Domain;

namespace Lykke.Service.Icon.Api.Core.Services
{
    public interface IBalanceService
    {
        Task<bool> BeginObservationIfNotObservingAsync(string address);

        Task<bool> EndObservationIfObservingAsync(string address);

        Task<(IEnumerable<AddressBalance> balances, string continuationToken)> GetTransferableBalancesAsync(int take, string continuationToken);
    }
}
