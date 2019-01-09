using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract;
using Lykke.Service.BlockchainApi.Contract.Balances;
using Lykke.Service.Icon.Api.Core;
using Lykke.Service.Icon.Api.Core.Services;
using Lykke.Service.Icon.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.Icon.Api.Controllers
{
    [PublicAPI, Route("api/balances")]
    public class BalancesController : Controller
    {
        private readonly IBalanceService _balanceService;

        public BalancesController(
            IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [HttpPost("{address}/observation")]
        public async Task<ActionResult> AddAddressToObservationList(
            AddressRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            if (await _balanceService.BeginObservationIfNotObservingAsync(address))
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }
        
        [HttpDelete("{address}/observation")]
        public async Task<ActionResult> DeleteAddressFromObservationList(
            AddressRequest request)
        {
            var address = request.Address.ToLowerInvariant();
            
            if (await _balanceService.EndObservationIfObservingAsync(address))
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<WalletBalanceContract>>> GetBalanceList(
            PaginationRequest request)
        {
            var (balances, continuation) = await _balanceService.GetTransferableBalancesAsync
            (
                request.Take,
                request.Continuation
            );

            return new PaginationResponse<WalletBalanceContract>
            {
                Continuation = continuation,
                Items = balances.Select(x => new WalletBalanceContract
                {
                    Address = x.Address,
                    AssetId = Constants.AssetId,
                    Balance = x.Amount,
                    Block = x.BlockNumber
                }).ToList()
            };
        }
    }
}
