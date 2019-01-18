using System.Threading.Tasks;
using Lykke.Job.Icon.Settings;
using Lykke.Quintessence;

namespace Lykke.Job.Icon
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
            return JobStarter
                .StartAsync<Startup, IconJobSettings>(IsDebug);
        }
    }
}
