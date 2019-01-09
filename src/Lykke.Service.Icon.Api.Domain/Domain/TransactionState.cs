using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Domain
{
    public enum TransactionState
    {
        Built = 0,
        InProgress = 1,
        Completed = 2,
        Failed = 3,
        Deleted = 4
    }
}
