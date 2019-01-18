using JetBrains.Annotations;
using Lykke.Quintessence.Settings;

namespace Lykke.Job.Icon.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class IconJobSettings : JobSettings
    {
        public bool IsMainNet { get; set; }
    }
}
