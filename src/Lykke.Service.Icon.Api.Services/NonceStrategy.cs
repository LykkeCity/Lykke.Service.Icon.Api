using Lykke.Quintessence.Domain.Services;
using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Service.Icon.Api.Services
{
    public class NonceService : INonceService
    {
        //just a fake
        public Task<BigInteger> GetNextNonceAsync(string address)
        {
            return Task.FromResult(BigInteger.One);
        }
    }
}
