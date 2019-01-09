using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Domain
{
    public struct AddressBalance
    {
        public string Address { get; set; }
        public string Amount { get; set; }
        public long BlockNumber { get; set; }
    }
}
