using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;
using Lykke.Quintessence.Validators;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class WhitelistAddressRequestValidator : Lykke.Quintessence.Validators.TransactionHistoryRequestValidator
    {
        public WhitelistAddressRequestValidator(
            IAddressService addressService) : base(addressService)
        {
        }
    }
}
