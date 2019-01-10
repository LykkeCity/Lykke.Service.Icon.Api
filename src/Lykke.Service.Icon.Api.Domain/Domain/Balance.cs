using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Domain
{
    public class Balance
    {
        public Balance(
            string address)
        {
            Address = address;
        }

        public Balance(
            string address,
            BigInteger amount,
            BigInteger blockNumber)

            : this(address)
        {
            Amount = amount;
            BlockNumber = blockNumber;
        }


        public string Address { get; }

        public BigInteger Amount { get; private set; }

        public BigInteger BlockNumber { get; private set; }


        public void OnUpdated(
            BigInteger amount,
            BigInteger blockNumber)
        {
            Amount = amount;
            BlockNumber = blockNumber;
        }
    }
}
