using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Icon.Api.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AddressRequest
    {
        [FromRoute]
        public string Address { get; set; }
    }
}
