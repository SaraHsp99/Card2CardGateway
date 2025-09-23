using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.Interfaces
{
    public interface IBankServiceFactory
    {
        IBankTransferService Resolve(string sourceCardNumber);
    }

}
