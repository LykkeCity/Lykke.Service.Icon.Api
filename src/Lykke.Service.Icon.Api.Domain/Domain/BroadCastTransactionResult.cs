using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Domain
{
    public class BroadcastTransactionResult
    {
        public class TransactionContext : BroadcastTransactionResult
        {
            public string TransactionHash { get; set; }
        }

        public class Error : BroadcastTransactionResult
        {
            public BroadcastTransactionError Type { get; set; }
        }
    }
}
