using Lykke.Service.Icon.Api.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Icon.Api.Core.Services
{
    public interface ITransactionHistoryService
    {
        Task<IEnumerable<TransactionReceipt>> GetIncomingHistoryAsync(string address, int requestTake, string requestAfterHash);

        Task<IEnumerable<TransactionReceipt>> GetOutgoingHistoryAsync(string address, int requestTake, string requestAfterHash);
    }
}
