using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class AddressRequestValidator : AbstractValidator<AddressRequest>
    {
        public AddressRequestValidator(
            IAddressService addressService)
        {
            Quintessence.Validators.Rules.AddressMustBeValid(RuleFor(x => x.Address), addressService);
        }
    }
}
