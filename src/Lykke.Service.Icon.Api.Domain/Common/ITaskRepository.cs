using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Icon.Api.Core.Common
{
    public interface ITaskRepository<T>
        where T : ITask<T>
    {
        Task CompleteAsync(
            T task);

        Task EnqueueAsync(
            T task);

        Task EnqueueAsync(
            T task,
            TimeSpan initialVisibilityDelay);

        Task<T> TryGetAsync(
            TimeSpan visibilityTimeout);
    }
}
