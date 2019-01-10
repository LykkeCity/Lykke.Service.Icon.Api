using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.Icon.Api.Core.Domain;

namespace Lykke.Service.Icon.Api.Core.Services
{
    public interface ITransactionService
    {
        [ItemNotNull]
        Task<BuildTransactionResult> BuildTransactionAsync(
            Guid transactionId,
            [NotNull] string from,
            [NotNull] string to,
            BigInteger transferAmount,
            bool includeFee);

        [ItemNotNull]
        Task<BroadcastTransactionResult> BroadcastTransactionAsync(
            Guid transactionId,
            [NotNull] string signedTxData);

        Task<bool> DeleteTransactionIfExistsAsync(
            Guid transactionId);

        Task<Transaction> TryGetTransactionAsync(
            Guid transactionId);
    }
}
