using System.Threading.Tasks;
using Lykke.Quintessence;
using Lykke.Service.Icon.Api.Settings;

namespace Lykke.Service.Icon.Api
{
    internal static class Program
    {
        #if DEBUG
        
        private const bool IsDebug = true;
        
        #else

        private const bool IsDebug = false;

        #endif
        
        private static Task Main()
        {
            return ApiStarter
                .StartAsync<Startup, IconApiSettings>(IsDebug);
        }
    }
}
