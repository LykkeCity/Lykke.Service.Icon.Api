using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;
using Lykke.Quintessence.Validators;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class TransactionHistoryRequestValidator : AbstractValidator<TransactionHistoryRequest>
    {
        public TransactionHistoryRequestValidator(
            IAddressService addressService)
        {
            RuleFor(x => x.Address)
                .AddressMustBeValid(addressService);

            RuleFor(x => x.Take)
                .GreaterThan(0);
        }
    }
}
