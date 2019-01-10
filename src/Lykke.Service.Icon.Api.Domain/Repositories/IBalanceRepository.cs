using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Icon.Api.Core.Domain;

namespace Lykke.Service.Icon.Api.Core.Repositories
{
    public interface IBalanceRepository
    {
        Task<bool> CreateIfNotExistsAsync(string address);

        Task<bool> DeleteIfExistsAsync(string address);

        Task<bool> ExistsAsync(string address);

        Task<Balance> TryGetAsync(string address);

        Task UpdateSafelyAsync(Balance balance);

        Task<(IEnumerable<Balance> Balances, string ContinuationToken)> GetAllTransferableBalancesAsync(int take, string continuationToken);
    }
}
