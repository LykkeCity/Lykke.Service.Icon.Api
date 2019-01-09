using Lykke.HttpClientGenerator;

namespace Lykke.Service.Icon.Api.Client
{
    /// <summary>
    /// Icon.Api API aggregating interface.
    /// </summary>
    public class Icon.ApiClient : IIcon.ApiClient
    {
        // Note: Add similar Api properties for each new service controller

        /// <summary>Inerface to Icon.Api Api.</summary>
        public IIcon.ApiApi Api { get; private set; }

        /// <summary>C-tor</summary>
        public Icon.ApiClient(IHttpClientGenerator httpClientGenerator)
        {
            Api = httpClientGenerator.Generate<IIcon.ApiApi>();
        }
    }
}
