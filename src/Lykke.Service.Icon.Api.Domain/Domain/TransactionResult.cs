using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Domain
{
    public class TransactionResult
    {
        public TransactionResult(
            BigInteger blockNumber,
            string error,
            bool isCompleted,
            bool isFailed)
        {
            BlockNumber = blockNumber;
            Error = error;
            IsCompleted = isCompleted;
            IsFailed = isFailed;
        }


        public BigInteger BlockNumber { get; }

        public string Error { get; }

        public bool IsCompleted { get; }

        public bool IsFailed { get; }
    }
}
