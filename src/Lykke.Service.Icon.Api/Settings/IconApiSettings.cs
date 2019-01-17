using JetBrains.Annotations;
using Lykke.Quintessence.Settings;

namespace Lykke.Service.Icon.Api.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class IconApiSettings : ApiSettings
    {
        public bool IsMainNet { get; set; }
    }
}
