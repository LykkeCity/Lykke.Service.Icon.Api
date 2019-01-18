using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.Quintessence.Domain;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.RpcClient;
using System.Numerics;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using TransactionResult = Lykke.Icon.Sdk.Data.TransactionResult;

namespace Lykke.Service.Icon.Api.Services
{
    public class GetTransactionReceiptsStrategy : IGetTransactionReceiptsStrategy
    {
        private readonly ITryGetTransactionErrorStrategy _tryGetTransactionErrorStrategy;
        private readonly IIconService _iconService;

        public GetTransactionReceiptsStrategy(
            IIconService iconService,
            ITryGetTransactionErrorStrategy tryGetTransactionErrorStrategy)
        {
            _tryGetTransactionErrorStrategy = tryGetTransactionErrorStrategy;
            _iconService = iconService;
        }


        public async Task<IEnumerable<TransactionReceipt>> ExecuteAsync(
            BigInteger blockNumber)
        {

            var block = await _iconService.GetBlock(blockNumber);
            var transactions = block?.GetTransactions();

            var transactionReceipts = new List<TransactionReceipt>();

            if (transactions != null)
            {
                foreach (var blockTransaction in transactions)
                {
                    var txHash = blockTransaction.GetTxHash().ToHexString(true);
                    var bytes = new Bytes(txHash);
                    var txResult = await _iconService.GetTransactionResult(bytes);
                    
                    if (txResult == null)
                        throw new InvalidOperationException(
                            $"Block[{blockNumber}] contains confirmed transaction " +
                            $"which results can not be retrieved as transaction results: txHash[{txHash}]");

                    var error = await _tryGetTransactionErrorStrategy.ExecuteAsync(txResult.GetStatus(), txHash);
                    if (error != null)
                        continue;

                    if (blockTransaction.GetTo() != null &&
                        blockTransaction.GetTo().GetPrefix() == AddressPrefix.FromString(AddressPrefix.Contract))
                    {
                        transactionReceipts.AddRange
                        (
                            GetInternalTransactionReceipts
                            (
                                blockTransaction.GetTxHash().ToHexString(true),
                                txResult,
                                block.GetTimestamp()
                            )
                        );
                    }

                    var transactionTransferValue = blockTransaction.GetValue();

                    if (transactionTransferValue != null &&
                        transactionTransferValue != BigInteger.Zero)
                        transactionReceipts.Add(new TransactionReceipt
                        (
                            amount: transactionTransferValue.Value,
                            blockNumber: blockNumber,
                            from: blockTransaction.GetFrom().ToString(),
                            hash: blockTransaction.GetTxHash().ToHexString(true),
                            index: 0,
                            timestamp: block.GetTimestamp(),
                            to: blockTransaction.GetTo().ToString()
                        ));
                }
            }

            return transactionReceipts;
        }

        private IEnumerable<TransactionReceipt> GetInternalTransactionReceipts(
            string txHash,
            TransactionResult txResult,
            BigInteger timestamp)
        {
            var result = new List<TransactionReceipt>();
            var eventLog = txResult.GetEventLogs();
            var valueTransferTraces = eventLog.Where(log =>
            {
                var indexed = log?.GetIndexed();
                return indexed?.FirstOrDefault()
                           ?.ToString() == "ICXTransfer(Address,Address,int)"
                       && indexed.Count == 4;
            }).Select(log => log.GetIndexed());

            var index = 0;

            foreach (var eventItems in valueTransferTraces)
            {
                var from = eventItems[1].ToString();
                var to = eventItems[2].ToString();
                var value = eventItems[3].ToInteger();

                result.Add(new TransactionReceipt
                (
                    amount: value,
                    blockNumber: txResult.GetBlockHeight(),
                    from: from,
                    hash: txHash,
                    index: index++,
                    timestamp: timestamp,
                    to: to
                ));
            }

            return result;
        }
    }
}
