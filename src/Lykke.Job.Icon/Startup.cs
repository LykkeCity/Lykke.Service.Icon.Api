using JetBrains.Annotations;
using Lykke.Job.Icon.Modules;
using Lykke.Job.Icon.Settings;
using Lykke.Quintessence;
using Lykke.Sdk;

namespace Lykke.Job.Icon
{
    [UsedImplicitly]
    public class Startup : JobStartupBase<IconJobSettings>
    {
        protected override string IntegrationName
            => "Rootstock";

        protected override void RegisterAdditionalModules(
            IModuleRegistration modules)
        {
            modules.RegisterModule<IconJobModule>();
            
            base.RegisterAdditionalModules(modules);
        }
    }
}
