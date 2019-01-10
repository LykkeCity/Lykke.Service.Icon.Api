using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Domain
{
    public class Transaction
    {
        public BigInteger Amount { get; set; }

        public long? BlockNumber { get; set; }

        public TransactionState State { get; set; }

        public DateTime BuiltOn { get; set; }

        public DateTime? CompletedOn { get; set; }

        public string Hash { get; set; }

        public Guid TransactionId { get; set; }

        public BigInteger StepAmount { get; set; }

        public BigInteger StepPrice { get; set; }

        public string Error { get; set; }
    }
}
