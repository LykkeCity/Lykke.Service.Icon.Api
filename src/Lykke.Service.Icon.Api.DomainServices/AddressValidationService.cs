using Lykke.Service.Icon.Api.Core.Services;

namespace Lykke.Service.Icon.Api.DomainServices
{
    public class AddressValidationService : IAddressValidationService
    {
        public AddressValidationService()
        {
        }

        public bool IsAddressValid(string address)
        {
            bool isAddressValid = Lykke.Icon.Sdk.Crypto.IconKeys.IsValidAddress(address);

            return isAddressValid;
        }
    }
}
