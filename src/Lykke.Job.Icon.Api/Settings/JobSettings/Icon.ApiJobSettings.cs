namespace Lykke.Job.Icon.Api.Settings.JobSettings
{
    public class IconApiJobSettings
    {
        public DbSettings Db { get; set; }
        public AzureQueueSettings AzureQueue { get; set; }
        public RabbitMqSettings Rabbit { get; set; }
    }
}
