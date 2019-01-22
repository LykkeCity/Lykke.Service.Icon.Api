using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class BlacklistAddressRequestValidator : AbstractValidator<BlacklistAddressRequest>
    {
        public BlacklistAddressRequestValidator(
            IAddressService addressService)
        {
            Quintessence.Validators.Rules.AddressMustBeProperlyFormatted(RuleFor(x => x.Address), addressService);

            RuleFor(x => x.BlacklistingReason)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}
