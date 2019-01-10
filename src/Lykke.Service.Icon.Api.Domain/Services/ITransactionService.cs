using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Icon.Api.Core.Domain;

namespace Lykke.Service.Icon.Api.Core.Services
{
    public interface ITransactionService
    {
        Task<BuildTransactionResult> BuildTransactionAsync(Guid transactionId, string @from, string to, BigInteger amount, bool includeFee);

        Task<BroadcastTransactionResult> BroadcastTransactionAsync(Guid transactionId, string signedTxData);

        Task<bool> DeleteTransactionIfExistsAsync(Guid requestTransactionId);

        Task<Transaction> TryGetTransactionAsync(Guid requestTransactionId);
    }
}
