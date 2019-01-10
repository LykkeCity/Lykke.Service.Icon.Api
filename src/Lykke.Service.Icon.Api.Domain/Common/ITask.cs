using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Icon.Api.Core.Common
{
    public interface ITask<out T>
        where T : ITask<T>
    {
        string MessageId { get; }

        string PopReceipt { get; }


        T WithMessageIdAndPopReceipt(
            string messageId,
            string popReceipt);
    }
}
