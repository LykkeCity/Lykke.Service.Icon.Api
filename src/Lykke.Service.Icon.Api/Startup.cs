using JetBrains.Annotations;
using Lykke.Quintessence;
using Lykke.Sdk;
using Lykke.Service.Icon.Api.Modules;
using Lykke.Service.Icon.Api.Settings;

namespace Lykke.Service.Icon.Api
{
    [UsedImplicitly]
    public class Startup : ApiStartupBase<IconApiSettings>
    {
        protected override string IntegrationName
            => "Icon";

        protected override void RegisterAdditionalModules(
            IModuleRegistration modules)
        {
            modules.RegisterModule<IconApiModule>();
            
            base.RegisterAdditionalModules(modules);
        }
    }
}
