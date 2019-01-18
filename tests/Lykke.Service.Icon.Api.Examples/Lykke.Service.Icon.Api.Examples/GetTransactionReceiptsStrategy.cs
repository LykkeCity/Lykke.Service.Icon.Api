using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Lykke.Quintessence.Domain.Services;
using Xunit;
using System.Numerics;
using System.Text.RegularExpressions;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Service.Icon.Api.Examples
{
    public class GetTransactionReceiptsStrategy : BaseTest
    {
        [Fact]
        public async Task ExecuteAsync__Get_Internal_Transaction_As_Recipients__Result_Is_Correct()
        {
            var getTransactionReceiptsStrategy = _container.Resolve<IGetTransactionReceiptsStrategy>();
            var number = 30_855;
            var expectedAmount = BigInteger.Parse("500000000000000000");

            var txReceipts = await getTransactionReceiptsStrategy.ExecuteAsync(number);
            var internalTransfer = txReceipts.FirstOrDefault();

            Assert.True(internalTransfer != null);
            Assert.True(internalTransfer.Amount == expectedAmount);
        }

        [Fact]
        public async Task ExecuteAsync__Get_Transaction_With_Error__Result_Is_Correct()
        {
            var getTransactionReceiptsStrategy = _container.Resolve<IGetTransactionReceiptsStrategy>();
            var number = 42_349;

            var txReceipts = await getTransactionReceiptsStrategy.ExecuteAsync(number);
            var internalTransfer = txReceipts.FirstOrDefault();

            Assert.True(internalTransfer == null);
        }

        [Fact]
        public async Task ExecuteAsync__Get_Transaction_With_Fake_Transfer__Result_Is_Correct()
        {
            var getTransactionReceiptsStrategy = _container.Resolve<IGetTransactionReceiptsStrategy>();
            var number = 42_368;

            var txReceipts = await getTransactionReceiptsStrategy.ExecuteAsync(number);
            var internalTransfer = txReceipts.FirstOrDefault();

            Assert.True(internalTransfer == null);
        }

        [Fact]
        public async Task ExecuteAsync__Get_Transaction_Common_Transfer__Result_Is_Correct()
        {
            var getTransactionReceiptsStrategy = _container.Resolve<IGetTransactionReceiptsStrategy>();
            var number = 43_376;
            var expectedAmount = BigInteger.Parse("7450000000000000000");

            var txReceipts = await getTransactionReceiptsStrategy.ExecuteAsync(number);
            var internalTransfer = txReceipts.FirstOrDefault();

            Assert.True(internalTransfer != null);
            Assert.True(internalTransfer.Amount == expectedAmount);
        }
    }
}
