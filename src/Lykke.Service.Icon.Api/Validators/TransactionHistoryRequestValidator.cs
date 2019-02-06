using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class TransactionHistoryRequestValidator : Lykke.Quintessence.Validators.TransactionHistoryRequestValidator
    {
        public TransactionHistoryRequestValidator(
            IAddressService addressService) : base(addressService)
        {
        }
    }
}
