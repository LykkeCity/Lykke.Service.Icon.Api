using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Icon.Api.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class IconApiSettings
    {
        public DbSettings Db { get; set; }
    }
}
