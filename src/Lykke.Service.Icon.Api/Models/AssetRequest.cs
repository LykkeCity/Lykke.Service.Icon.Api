using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Icon.Api.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AssetRequest
    {
        [FromRoute]
        public string AssetId { get; set; }
    }
}
