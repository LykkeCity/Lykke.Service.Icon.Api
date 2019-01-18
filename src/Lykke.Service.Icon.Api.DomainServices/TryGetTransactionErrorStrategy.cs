using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.RpcClient;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Lykke.Service.Icon.Api.Services
{
    public class TryGetTransactionErrorStrategy : ITryGetTransactionErrorStrategy
    {
        public TryGetTransactionErrorStrategy()
        {
        }

        //https://github.com/icon-project/icon-rpc-server/blob/master/docs/icon-json-rpc-v3.md#icx_gettransactionresult
        //states that status either 1 on success or 0 on failure.
        public async Task<string> ExecuteAsync(
            BigInteger? transactionStatus,
            string transactionHash)
        {
            if (transactionStatus.HasValue)
            {
                switch ((int)transactionStatus.Value)
                {
                    case 0:
                        return "Transaction failed";
                    case 1:
                        return null;
                    default:
                        throw new Exception($"Transaction [{transactionHash}] has unexpected [{transactionStatus.Value}] status.");
                }
            }

            throw new Exception($"Transaction [{transactionHash}] has unexpected null status. (Icon does not support such behaviour)");
        }
    }
}
