using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Domain.Enums
{
    public enum TransactionStatus
    {
        Unknown = 0,
        Success = 1,
        InsufficientFunds = 2,
        InvalidPin = 3,
        InternalError = 4
    }

}
