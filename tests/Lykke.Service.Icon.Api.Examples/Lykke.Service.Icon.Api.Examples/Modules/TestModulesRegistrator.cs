using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Lykke.Quintessence.Modules;
using Lykke.Quintessence.Settings;
using Lykke.Service.Icon.Api.Modules;
using Lykke.Service.Icon.Api.Settings;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.Icon.Api.Examples.Modules
{
    public static class TestModulesRegistrator
    {
        public async static Task<IContainer> GetContainer(string settingsUrl)
        {
            var reader = new Lykke.SettingsReader.SettingsServiceReloadingManager<AppSettings<IconApiSettings>>(settingsUrl, (x) => { });
            var reloadingManager = await reader.Reload();
            var apiModule = new ApiModule<IconApiSettings>(reader);
            var iconApiModule = new IconApiModule(reader);
            var services = new ServiceCollection();
            ContainerBuilder builder = new ContainerBuilder();
            services.AddHttpClient();
            builder.
                RegisterInstance(reader)
                .As<IReloadingManager<AppSettings<IconApiSettings>>>()
                .SingleInstance();
            builder.RegisterModule(iconApiModule);
            builder.RegisterModule(apiModule);
            builder.Populate(services);
            var container = builder.Build();

            return container;
        }
    }
}
