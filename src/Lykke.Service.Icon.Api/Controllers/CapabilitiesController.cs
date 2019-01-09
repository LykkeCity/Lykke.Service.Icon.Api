using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Icon.Api.Controllers
{
    [PublicAPI, Route("api/capabilities")]
    public class CapabilitiesController : Controller
    {
        private static readonly CapabilitiesResponse CapabilitiesResponse = 
            new CapabilitiesResponse();

        
        [HttpGet]
        public ActionResult<CapabilitiesResponse> Get()
        {
            return CapabilitiesResponse;
        }
    }
}
