using System;
using System.Threading.Tasks;
using Autofac;
using Lykke.Quintessence.Domain.Services;
using Xunit;
using System.Numerics;
using System.Text.RegularExpressions;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Service.Icon.Api.Core.Helpers;

namespace Lykke.Service.Icon.Api.Examples
{
    public class BlockchainServiceIntegrationTest : BaseTest
    {
        [Fact]
        public async Task BuildTransactionAsync__Transaction_Is_Built__Transaction_Is_Correct()
        {
            var blockchainService = _container.Resolve<IBlockchainService>();
            string from = _secretWallet.GetAddress().ToString();
            string to = _toWallet.GetAddress().ToString();
            BigInteger amount = BigInteger.Parse("1000000000000000000");
            BigInteger gasAmount = BigInteger.Parse("1000000");
            BigInteger gasPrice = 1;
            var transaction = blockchainService.BuildTransaction(from, to, amount, gasAmount, gasPrice, 0);

            string txDataStr = System.Text.Encoding.UTF8.DecodeBase64(transaction);
            Regex regex = new Regex("timestamp\\.[^\\.]+\\.");
            txDataStr = regex.Replace(txDataStr, "timestamp.0x226c8.");
            var expectedItToBe =
                "icx_sendTransaction.from.hxe8db3bc33564f5b07cd52e37e2d762d3073a2c1d.nid.0x2.stepLimit.0xf4240.timestamp.0x226c8.to.hxa0c8aad540a8b85175b59d98585e0f79ffbc2502.value.0xde0b6b3a7640000.version.0x3";

            Assert.Equal(expectedItToBe, txDataStr);

        }

        [Fact]
        public async Task BroadcastTransactionAsync__Transaction_Is_Broadcasted__Transaction_Is_Sent()
        {
            var blockchainService = _container.Resolve<IBlockchainService>();
            string from = _secretWallet.GetAddress().ToString();
            string to = _toWallet.GetAddress().ToString();
            BigInteger amount = BigInteger.Parse("100000000000000");
            BigInteger gasAmount = BigInteger.Parse("1000000");
            BigInteger gasPrice = 1;
            var transaction = blockchainService.BuildTransaction(from, to, amount, gasAmount, gasPrice, 0);

            string txDataStr = System.Text.Encoding.UTF8.DecodeBase64(transaction);
            var iconTransaction = TransactionDeserializer.Deserialize(txDataStr);
            var signedTransaction = new SignedTransaction(iconTransaction, _secretWallet);
            var properties = signedTransaction.GetProperties();
            var transactionProperties = signedTransaction.GetTransactionProperties();
            var transactionHash = SignedTransaction.GetTransactionHash(transactionProperties);
            var serializedSignedTransaction = SignedTransaction.Serialize(properties);
            string serializedSignedTransactionBase64 = System.Text.Encoding.UTF8.EncodeBase64(serializedSignedTransaction);

            var expectedTxHash = (new Bytes(transactionHash)).ToHexString(true);
            var txHash = await blockchainService.BroadcastTransactionAsync(serializedSignedTransactionBase64);

            Assert.Equal(expectedTxHash, txHash);
        }

        [Fact]
        public async Task GetTransactionResultAsync__Transaction_Has_Been_Executed__Transaction_Result_Received()
        {
            var blockchainService = _container.Resolve<IBlockchainService>();
            string txHash = "0xefe60b8bf5d8de6c26eca068b146d0b018c791865a65a117047e6e64dad7ee95";

            var transactionResult = await blockchainService.GetTransactionResultAsync(txHash);

            Assert.True(transactionResult.BlockNumber == 3_596_187);
            Assert.True(transactionResult.IsCompleted);
            Assert.True(!transactionResult.IsFailed);
            Assert.True(transactionResult.Error == null);
        }

        [Fact]
        public async Task GetBalanceAsync__Address_Has_Balance__Balance_Is_Higher_Than_Zero()
        {
            var blockchainService = _container.Resolve<IBlockchainService>();
            string from = _secretWallet.GetAddress().ToString();

            var transactionResult = await blockchainService.GetBalanceAsync(from);
            Assert.True(transactionResult > 0);
        }
    }
}
