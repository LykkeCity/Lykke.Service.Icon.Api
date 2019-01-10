using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Domain
{
    public class TransactionReceipt
    {
        public BigInteger Amount { get; set; }
        public string To { get; set; }
        public string Hash { get; set; }
        public string From { get; set; }
        public long Timestamp { get; set; }
    }
}
