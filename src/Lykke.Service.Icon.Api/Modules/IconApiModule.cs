using Autofac;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Blockchain;
using Lykke.Quintessence.Core.Crypto;
using Lykke.Quintessence.Core.DependencyInjection;
using Lykke.Quintessence.Core.Telemetry.DependencyInjection;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Domain.Services.DependencyInjection;
using Lykke.Quintessence.Domain.Services.Strategies;
using Lykke.Quintessence.Settings;
using Lykke.Service.Icon.Api.Services;
using Lykke.Service.Icon.Api.Settings;
using Lykke.SettingsReader;
using Multiformats.Hash;

namespace Lykke.Service.Icon.Api.Modules
{
    [UsedImplicitly]
    public class IconApiModule : Module
    {
        private readonly IReloadingManager<AppSettings<IconApiSettings>> _appSettings;

        public IconApiModule(
            IReloadingManager<AppSettings<IconApiSettings>> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(
            ContainerBuilder builder)
        {
            var networkId = _appSettings.CurrentValue.Api.IsMainNet ? 1 : 2;

            builder
                .UseAITelemetryConsumer()
                .UseAssetService<IconAssetService>()
                .UseChainId(networkId);

            builder
                .UseAITelemetryConsumer()
                .RegisterType<BlockchainService>()
                .As<IBlockchainService>()
                .SingleInstance();

            builder
                .UseAITelemetryConsumer()
                .RegisterType<WalletGenerator>()
                .As<IWalletGenerator>()
                .SingleInstance();

            builder
                .UseAITelemetryConsumer()
                .RegisterType<KeyGenerator>()
                .As<IKeyGenerator>()
                .SingleInstance();

            builder
                .UseAITelemetryConsumer()
                .RegisterType<HashCalculator>()
                .As<IHashCalculator>()
                .WithParameter("hashType", HashType.SHA3_256)
                .SingleInstance();

            builder
                .UseAITelemetryConsumer()
                .RegisterType<KeyGenerator>()
                .As<IKeyGenerator>()
                .SingleInstance();

            builder
                .UseAITelemetryConsumer()
                .RegisterType<IconAddressService>()
                .As<IAddressService>()
                .SingleInstance();

            builder
                .UseAITelemetryConsumer()
                .RegisterType<GetTransactionReceiptsStrategy>()
                .As<IGetTransactionReceiptsStrategy>()
                .SingleInstance();

            builder
                .UseAITelemetryConsumer()
                .RegisterType<TryGetTransactionErrorStrategy>()
                .As<ITryGetTransactionErrorStrategy>()
                .SingleInstance();
        }
    }
}
