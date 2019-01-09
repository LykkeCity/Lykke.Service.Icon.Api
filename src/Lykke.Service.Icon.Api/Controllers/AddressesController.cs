using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract.Addresses;
using Lykke.Service.Icon.Api.Core.Services;
using Lykke.Service.Icon.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lykke.Service.Icon.Api.Controllers
{
    [PublicAPI, Route("/api/addresses")]
    public class AddressesController : Controller
    {
        private readonly IAddressValidationService _addressValidationService;

        public AddressesController(IAddressValidationService addressValidationService)
        {
            _addressValidationService = addressValidationService;
        }


        [HttpGet("{address}/validity")]
        public async Task<ActionResult<AddressValidationResponse>> GetAddressValidity(
            string address)
        {
            bool isAddressValid = _addressValidationService.IsAddressValid(address);

            return new AddressValidationResponse
            {
                IsValid = isAddressValid
            };
        }

        #region Not Implemented Endpoints

        [HttpGet("{address}/balance")]
        public ActionResult<AddressValidationResponse> GetBalance(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);

        [HttpGet("{address}/explorer-url")]
        public ActionResult<AddressValidationResponse> GetExplorerUrl(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);

        [HttpGet("{address}/underlying")]
        public ActionResult<AddressValidationResponse> GetUnderlyingAddress(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);

        [HttpGet("{address}/virtual")]
        public ActionResult<AddressValidationResponse> GetVirtualAddress(
            AddressRequest address)
                => StatusCode(StatusCodes.Status501NotImplemented);

        #endregion
    }
}
