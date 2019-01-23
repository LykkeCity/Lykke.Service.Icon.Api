using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Domain;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.Domain.Services.Utils;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Icon.Api.Core.Helpers;
using TransactionResult = Lykke.Quintessence.Domain.TransactionResult;

namespace Lykke.Service.Icon.Api.Services
{
    public class BlockchainService : IBlockchainService
    {
        private readonly SettingManager<int> _confirmationLevel;
        private readonly SettingManager<string, (BigInteger, BigInteger)> _gasPriceRange;
        private readonly IChainId _chainId;
        private readonly IIconService _iconService;
        private readonly IGetTransactionReceiptsStrategy _getTransactionReceiptsStrategy;
        private readonly ITryGetTransactionErrorStrategy _tryGetTransactionErrorStrategy;


        public BlockchainService(
            IIconService iconService,
            IGetTransactionReceiptsStrategy getTransactionReceiptsStrategy,
            ITryGetTransactionErrorStrategy tryGetTransactionErrorStrategy,
            DefaultBlockchainService.Settings settings,
            IChainId chainId)
        {
            _chainId = chainId;
            _iconService = iconService;
            _getTransactionReceiptsStrategy = getTransactionReceiptsStrategy;
            _tryGetTransactionErrorStrategy = tryGetTransactionErrorStrategy;

            _confirmationLevel = new SettingManager<int>
            (
                settings.ConfirmationLevel,
                TimeSpan.FromMinutes(1)
            );

            _gasPriceRange = new SettingManager<string, (BigInteger, BigInteger)>
            (
                settings.GasPriceRange,
                ParseGasPriceRange,
                TimeSpan.FromMinutes(1)
            );
        }

        public virtual async Task<string> BroadcastTransactionAsync(
            string signedTxData)
        {
            string txDataStr = System.Text.Encoding.UTF8.DecodeBase64(signedTxData);
            var transaction = SignedTransaction.Deserialize(txDataStr);
            var props = transaction.GetTransactionProperties();
            var transactionHash = SignedTransaction.GetTransactionHash(props);
            var txHashBytes = new Bytes(transactionHash);

            var existingHash = await WrapRpcErrorWhenTransactionIsNotYetConfirmedAsync(async () =>
            {
                if (await _iconService.GetTransactionResult(txHashBytes) != null)
                {
                    return txHashBytes.ToHexString(true);
                }

                return null;
            });

            var receivedBytes = await _iconService.SendTransaction(transaction);

            for (var i = 0; i < 10; i++)
            {
                existingHash = await WrapRpcErrorWhenTransactionIsNotYetConfirmedAsync(async () =>
                {
                    if (await _iconService.GetTransactionResult(receivedBytes) != null)
                    {
                        return receivedBytes.ToHexString(true);
                    }

                    return null;
                });

                if (!string.IsNullOrEmpty(existingHash))
                {
                    return existingHash;
                }
                else
                {
                    await Task.Delay(1000);
                }
            }

            throw new Exception
            (
                $"Transaction [{receivedBytes.ToHexString(true)}] has been broadcasted, but did not appear in mempool in the specified period of time."
            );
        }

        public virtual async Task<string> BuildTransactionAsync(
            string from,
            string to,
            BigInteger amount,
            BigInteger gasAmount,
            BigInteger gasPrice)
        {
            var timestamap = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(10)).ToUnixTimeSeconds() * 1_000_000L;
            var transaction = Lykke.Icon.Sdk.TransactionBuilder
                .CreateBuilder()
                .Nid(_chainId.Value)
                .StepLimit(gasAmount)
                .From(new Address(from))
                .To(new Address(to))
                .Value(amount)
                .Timestamp(timestamap)
                //.Nonce(await _nonceService.GetNextNonceAsync(from))
                .Build();
            var rpcObject = SignedTransaction.GetTransactionProperties(transaction);
            string serializedTransaction = TransactionSerializer.Serialize(rpcObject);
            var base64 = System.Text.Encoding.UTF8.EncodeBase64(serializedTransaction);

            return base64;
        }

        public virtual async Task<BigInteger> EstimateGasPriceAsync()
        {
            var (minGasPrice, maxGasPrice) = await _gasPriceRange.GetValueAsync();

            return minGasPrice + (maxGasPrice - minGasPrice) / 2;

            //var estimatedGasPrice = await _apiClient.GetGasPriceAsync();

            //if (estimatedGasPrice >= maxGasPrice)
            //{
            //    return maxGasPrice;
            //}
            //else if (estimatedGasPrice <= minGasPrice)
            //{
            //    return minGasPrice;
            //}
            //else
            //{
            //    return estimatedGasPrice;
            //}
        }

        public virtual Task<BigInteger> GetBalanceAsync(
            string address)
        {
            var iconAddress = new Address(address);

            return _iconService.GetBalance(iconAddress);
        }

        //?)
        public virtual Task<BigInteger> GetBalanceAsync(
            string address,
            BigInteger blockNumber)
        {
            var iconAddress = new Address(address);

            return _iconService.GetBalance(iconAddress);
        }

        public virtual async Task<BigInteger> GetBestTrustedBlockNumberAsync()
        {
            var bestBlockNumber = await _iconService.GetLastBlock();
            var confirmationLevel = await _confirmationLevel.GetValueAsync();

            return bestBlockNumber.GetHeight() - confirmationLevel;
        }

        public virtual Task<IEnumerable<TransactionReceipt>> GetTransactionReceiptsAsync(
            BigInteger blockNumber)
        {
            return _getTransactionReceiptsStrategy.ExecuteAsync(blockNumber);
        }

        public virtual async Task<TransactionResult> GetTransactionResultAsync(
            string hash)
        {
            var hashBytes = new Bytes(hash);
            var transactionReceipt = await _iconService.GetTransactionResult(hashBytes);

            if (transactionReceipt != null && transactionReceipt?.GetBlockHash() != null)
            {
                var block = await _iconService.GetBlock(transactionReceipt?.GetBlockHash());
                var blockNumber = block.GetHeight();
                var error = await _tryGetTransactionErrorStrategy.ExecuteAsync(transactionReceipt.GetStatus(), hash);

                return new TransactionResult
                (
                    blockNumber: blockNumber,
                    error: error,
                    isCompleted: true,
                    isFailed: !string.IsNullOrEmpty(error)
                );
            }
            else
            {
                return new TransactionResult
                (
                    blockNumber: 0,
                    error: null,
                    isCompleted: false,
                    isFailed: false
                );
            }
        }

        public virtual async Task<bool> IsContractAsync(
            string address)
        {
            var addressIcon = new Address(address);

            return addressIcon.GetPrefix() == AddressPrefix.FromString(AddressPrefix.Contract);
        }

        public virtual Task<BigInteger?> TryEstimateGasAmountAsync(
            string from,
            string to,
            BigInteger amount)
        {
            var receiverAddress = new Address(to);
            if (receiverAddress.GetPrefix() == AddressPrefix.FromString(AddressPrefix.Contract))
                return Task.FromResult((BigInteger?)null);

            return Task.FromResult(new BigInteger?(100_000));//From Account to Account
        }

        private static (BigInteger, BigInteger) ParseGasPriceRange(
            string source)
        {
            var minAndMagGasPrices = source.Split('-');
            var minGasPrice = BigInteger.Parse(minAndMagGasPrices[0]);
            var maxGasPrice = BigInteger.Parse(minAndMagGasPrices[1]);

            return (minGasPrice, maxGasPrice);
        }

        private static async Task<T> WrapRpcErrorWhenTransactionIsNotYetConfirmedAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return await func();
            }
            catch (Lykke.Icon.Sdk.Transport.JsonRpc.RpcErrorException e)
            {
                if (e.Code != -32602 && e.Message != "Invalid params txHash")
                    throw;
            }

            return default(T);
        }
    }
}
