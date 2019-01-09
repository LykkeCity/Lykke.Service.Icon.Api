using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Domain
{
    public enum BroadCastTransactionError
    {
        AmountIsTooSmall = 0,
        BalanceIsNotEnough = 1,
        TransactionHasBeenBroadcasted = 2,
        TransactionHasBeenDeleted = 3,
        TransactionShouldBeRebuilt,
        OperationHasNotBeenFound
    }
}
