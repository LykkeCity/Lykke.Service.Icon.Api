using System;
using FluentValidation;
using FluentValidation.Validators;
using JetBrains.Annotations;
using Lykke.Icon.Sdk;
using Lykke.Service.BlockchainApi.Contract.Transactions;
using Lykke.Service.Icon.Api.Core.Helpers;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class BroadcastTransactionRequestValidator : AbstractValidator<BroadcastTransactionRequest>
    {
        public BroadcastTransactionRequestValidator()
        {
            Quintessence.Validators.Rules.TransactionIdMustBeNonEmptyGuid(RuleFor(x => x.OperationId));
            
            RuleFor(x => x.SignedTransaction)
                .NotNull()
                .Custom(ValidateSignedTransaction);
        }

        private void ValidateSignedTransaction(string signedTransaction, CustomContext context)
        {
            try
            {
                string txDataStr = System.Text.Encoding.UTF8.DecodeBase64(signedTransaction);
                var transaction = SignedTransaction.Deserialize(txDataStr);
                var props = transaction.GetProperties();
                var signature = props.GetItem("signature");

                if (string.IsNullOrEmpty(signature?.ToString()))
                    context.AddFailure("SignedTransaction", "Signature is empty");
            }
            catch (Exception e)
            {
                context.AddFailure("SignedTransaction", e.Message);
            }
        }
    }
}
