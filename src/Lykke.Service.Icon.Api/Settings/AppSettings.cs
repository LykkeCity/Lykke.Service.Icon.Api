using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Service.Icon.Api.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public IconApiSettings IconApiService { get; set; }
    }
}
