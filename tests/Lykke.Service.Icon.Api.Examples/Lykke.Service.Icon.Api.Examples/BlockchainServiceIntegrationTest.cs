using System;
using System.Threading.Tasks;
using Autofac;
using Lykke.Quintessence.Domain.Services;
using Xunit;
using System.Numerics;
using System.Text.RegularExpressions;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;

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
            var transaction = await blockchainService.BuildTransactionAsync(from, to, amount, gasAmount, gasPrice);

            Regex regex = new Regex("timestamp\\.[^\\.]+\\.");
            transaction = regex.Replace(transaction, "timestamp.0x226c8.");
            var expectedItToBe =
                "icx_sendTransaction.from.hx659c15230d21ffe9f4c841e2a33126596b459226.nid.0x2.stepLimit.0xf4240.timestamp.0x226c8.to.hxee69c2e20990f44ba4963303df81873c7d084283.value.0xde0b6b3a7640000.version.0x3";

            Assert.Equal(expectedItToBe, transaction);

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
            var transaction = await blockchainService.BuildTransactionAsync(from, to, amount, gasAmount, gasPrice);

            var iconTransaction = TransactionDeserializer.Deserialize(transaction);
            var signedTransaction = new SignedTransaction(iconTransaction, _secretWallet);
            var properties = signedTransaction.GetProperties();
            var transactionProperties = signedTransaction.GetTransactionProperties();
            var transactionHash = SignedTransaction.GetTransactionHash(transactionProperties);
            var serializedSignedTransaction = SignedTransaction.Serialize(properties);

            var expectedTxHash = (new Bytes(transactionHash)).ToHexString(true);
            var txHash = await blockchainService.BroadcastTransactionAsync(serializedSignedTransaction);

            Assert.Equal(expectedTxHash, txHash);
        }

        [Fact]
        public async Task GetTransactionResultAsync__Transaction_Has_Been_Executed__Transaction_Result_Received()
        {
            var blockchainService = _container.Resolve<IBlockchainService>();
            string txHash = "0x8e3c97d792762cf805c194541bfee7b09418fa315e99ff8f4cebbd368e330193";

            var transactionResult = await blockchainService.GetTransactionResultAsync(txHash);

            Assert.True(transactionResult.BlockNumber == 43379);
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
