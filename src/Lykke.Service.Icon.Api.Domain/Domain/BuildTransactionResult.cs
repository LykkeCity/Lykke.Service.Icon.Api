using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Domain
{
    public class BuildTransactionResult
    {
        public class TransactionContext : BuildTransactionResult
        {
            public string TransactionHash { get; set; }
        }

        public class Error : BuildTransactionResult
        {
            public BuildTransactionError Type { get; set; }
        }
    }
}
