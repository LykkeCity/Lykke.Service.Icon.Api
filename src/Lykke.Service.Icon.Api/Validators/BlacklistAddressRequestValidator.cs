using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class BlacklistAddressRequestValidator : Lykke.Quintessence.Validators.BlacklistAddressRequestValidator
    {
        public BlacklistAddressRequestValidator(
            IAddressService addressService) : base(addressService)
        {
        }
    }
}
