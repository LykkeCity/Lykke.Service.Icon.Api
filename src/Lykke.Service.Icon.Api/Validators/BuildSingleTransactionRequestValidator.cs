using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Service.BlockchainApi.Contract.Transactions;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class BuildSingleTransactionRequestValidator : AbstractValidator<BuildSingleTransactionRequest>
    {
        public BuildSingleTransactionRequestValidator(
            IAddressService addressService,
            IAssetService assetService)
        {
            Quintessence.Validators.Rules.AmountMustBeValid(RuleFor(x => x.Amount));

            Quintessence.Validators.Rules.AddressMustBeValid(RuleFor(x => x.FromAddress), addressService);
            
            Quintessence.Validators.Rules.TransactionIdMustBeNonEmptyGuid(RuleFor(x => x.OperationId));

            Quintessence.Validators.Rules.AddressMustBeValid(RuleFor(x => x.ToAddress), addressService);

            Quintessence.Validators.Rules.AssetMustBeValidAndSupported(RuleFor(x => x.AssetId), assetService);
        }
    }
}
