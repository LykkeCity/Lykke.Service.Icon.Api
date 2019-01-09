using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Icon.Api.Client 
{
    /// <summary>
    /// Icon.Api client settings.
    /// </summary>
    public class Icon.ApiServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
