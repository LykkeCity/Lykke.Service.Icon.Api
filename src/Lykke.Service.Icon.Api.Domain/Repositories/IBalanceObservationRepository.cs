using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Icon.Api.Core.Repositories
{
    public interface IBalanceObservationRepository
    {
        Task<bool> BeginObservationIfNotObservingAsync(string address);

        Task<bool> EndObservationIfNotObservingAsync(string address);
    }
}
