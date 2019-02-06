using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Service.BlockchainApi.Contract.Transactions;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class BuildSingleTransactionRequestValidator : 
        Lykke.Quintessence.Validators.BuildSingleTransactionRequestValidator
    {
        public BuildSingleTransactionRequestValidator(
            IAddressService addressService,
            IAssetService assetService) : base(addressService, assetService)
        {
        }
    }
}
