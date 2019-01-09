using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Icon.Api.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
