﻿using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Models;
using Lykke.Quintessence.Validators;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class OperationRequestValidator : AbstractValidator<TransactionRequest>
    {
        public OperationRequestValidator()
        {
            RuleFor(x => x.TransactionId)
                .TransactionIdMustBeNonEmptyGuid();
        }
    }
}
