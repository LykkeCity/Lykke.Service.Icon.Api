using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.BlockchainApi.Contract.Transactions;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class BroadcastTransactionRequestValidator : AbstractValidator<BroadcastTransactionRequest>
    {
        public BroadcastTransactionRequestValidator()
        {
            Quintessence.Validators.Rules.TransactionIdMustBeNonEmptyGuid(RuleFor(x => x.OperationId));
            
            RuleFor(x => x.SignedTransaction).NotNull();
        }
    }
}
