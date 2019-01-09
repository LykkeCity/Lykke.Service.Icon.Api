using Lykke.SettingsReader.Attributes;

namespace Lykke.Job.Icon.Api.Settings.JobSettings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
