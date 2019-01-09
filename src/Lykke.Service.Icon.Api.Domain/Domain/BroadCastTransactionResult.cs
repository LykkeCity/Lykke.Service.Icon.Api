using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Domain
{
    public class BroadCastTransactionResult
    {
        public class TransactionContext
        {
            public string TransactionHash { get; set; }
        }

        public class Error
        {
            public BuildTransactionError Error { get; set; }
        }
    }
}
