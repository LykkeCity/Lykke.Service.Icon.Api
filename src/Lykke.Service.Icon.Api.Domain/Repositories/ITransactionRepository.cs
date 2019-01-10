using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.Icon.Api.Core.Domain;

namespace Lykke.Service.Icon.Api.Core.Repositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(
            [NotNull] Transaction transaction);

        [ItemCanBeNull]
        Task<Transaction> TryGetAsync(
            Guid transactionId);

        Task UpdateAsync(
            [NotNull] Transaction transaction);
    }
}
