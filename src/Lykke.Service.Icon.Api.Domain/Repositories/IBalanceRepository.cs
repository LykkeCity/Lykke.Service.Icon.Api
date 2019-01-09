using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Icon.Api.Core.Domain;

namespace Lykke.Service.Icon.Api.Core.Repositories
{
    public interface IBalanceRepository
    {
        Task<(IEnumerable<AddressBalance> balances, string continuationToken)> GetTransferableBalancesAsync(int take, string continuationToken);
    }
}
