using JetBrains.Annotations;

namespace Lykke.Service.Icon.Api.Client
{
    /// <summary>
    /// Icon.Api client interface.
    /// </summary>
    [PublicAPI]
    public interface IIcon.ApiClient
    {
        // Make your app's controller interfaces visible by adding corresponding properties here.
        // NO actual methods should be placed here (these go to controller interfaces, for example - IIcon.ApiApi).
        // ONLY properties for accessing controller interfaces are allowed.

        /// <summary>Application Api interface</summary>
        IIcon.ApiApi Api { get; }
    }
}
