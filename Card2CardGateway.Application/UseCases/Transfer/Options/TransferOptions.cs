using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.Options
{
    public class TransferOptions
    {
        public int MinAmount { get; set; } = 1000;
        public int MaxAmount { get; set; } = 10_000_000;
    }
}
