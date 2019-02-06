using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class AddressRequestValidator : Lykke.Quintessence.Validators.AddressRequestValidator
    {
        public AddressRequestValidator(
            IAddressService addressService) : base(addressService)
        {
        }
    }
}
