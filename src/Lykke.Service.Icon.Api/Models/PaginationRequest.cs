using JetBrains.Annotations;

namespace Lykke.Service.Icon.Api.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PaginationRequest
    {
        public PaginationRequest()
        {
            Continuation = string.Empty;
        }

        public string Continuation { get; set; }

        public int Take { get; set; }
    }
}
