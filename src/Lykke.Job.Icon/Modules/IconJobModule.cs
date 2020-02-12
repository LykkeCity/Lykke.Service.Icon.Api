using System.Net.Http;
using Autofac;
using JetBrains.Annotations;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Transport.Http;
using Lykke.Job.Icon.Settings;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Core.Crypto;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Core.Telemetry.DependencyInjection;
using Lykke.Quintessence.DependencyInjection;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Domain.Services.DependencyInjection;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.Settings;
using Lykke.Service.Icon.Api.Services;
using Lykke.SettingsReader;
using Multiformats.Hash;

namespace Lykke.Job.Icon.Modules
{
    [UsedImplicitly]
    public class IconJobModule : Module
    {
        private readonly IReloadingManager<AppSettings<IconJobSettings>> _appSettings;

        public IconJobModule(
            IReloadingManager<AppSettings<IconJobSettings>> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(
            ContainerBuilder builder)
        {
            var networkId = _appSettings.CurrentValue.Job.IsMainNet ? 1 : 2;

            var settings = new DefaultBlockchainService.Settings
            {
                ConfirmationLevel = _appSettings.Nested(x => x.Job.ConfirmationLevel),
                GasPriceRange = _appSettings.Nested(x => x.Job.GasPriceRange)
            };

            builder
                .RegisterInstance(settings)
                .SingleInstance();
            builder.UseChainId(networkId);

            builder
                .UseAITelemetryConsumer()
                .RegisterType<BlockchainService>()
                .As<IBlockchainService>()
                .SingleInstance();

            builder
                .RegisterType<NonceService>()
                .As<INonceService>()
                .SingleInstance();

            builder
                .RegisterInstance(new HttpProvider(
                    new HttpClient(),
                    _appSettings.CurrentValue.Job.RpcNode.ApiUrl))
                .As<IProvider>()
                .SingleInstance();

            builder
                .RegisterType<IconService>()
                .As<IIconService>()
                .SingleInstance();

            builder
                .RegisterType<WalletGenerator>()
                .As<IWalletGenerator>()
                .SingleInstance();

            builder
                .RegisterType<KeyGenerator>()
                .As<IKeyGenerator>()
                .SingleInstance();

            builder
                .RegisterInstance(HashCalculator.Sha3256)
                .As<IHashCalculator>()
                .SingleInstance();

            builder
                .RegisterType<KeyGenerator>()
                .As<IKeyGenerator>()
                .SingleInstance();

            builder
                .UseAITelemetryConsumer()
                .RegisterType<IconAddressService>()
                .As<IAddressService>()
                .SingleInstance();

            builder
                .RegisterType<GetTransactionReceiptsStrategy>()
                .As<IGetTransactionReceiptsStrategy>()
                .SingleInstance();

            builder
                .RegisterType<TryGetTransactionErrorStrategy>()
                .As<ITryGetTransactionErrorStrategy>()
                .SingleInstance();

            builder
                .UseQueueConsumers
                (
                    _appSettings.CurrentValue.Job.BalanceMonitoringMaxDegreeOfParallelism,
                    _appSettings.CurrentValue.Job.BlockchainIndexationMaxDegreeOfParallelism,
                    _appSettings.CurrentValue.Job.TransactionMonitoringMaxDegreeOfParallelism
                )
                .AddBalanceMonitoringQueueConsumer()
                .AddBlockchainIndexationQueueConsumer()
                .AddTransactionMonitoringQueueConsumer();
        }
    }
}
