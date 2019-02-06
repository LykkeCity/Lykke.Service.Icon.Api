using System;
using FluentValidation;
using JetBrains.Annotations;
using Lykke.Quintessence.Core.Utils;
using Lykke.Quintessence.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Lykke.Service.Icon.Api.Validators
{
    [UsedImplicitly]
    public class PaginationRequestValidator : Lykke.Quintessence.Validators.PaginationRequestValidator
    {
        public PaginationRequestValidator()
        {
        }
    }
}
