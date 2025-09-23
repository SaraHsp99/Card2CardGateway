using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.DTO
{
    public class TransferRequestDto
    {
        public long Amount { get; set; }
        public string SourceCardNumber { get; set; } = default!;
        public string DestinationCardNumber { get; set; } = default!;
        public string Pin2 { get; set; } = default!;  
        public string Cvv2 { get; set; } = default!;
        public string ExpiredDate { get; set; } = default!;
        public Guid RequestTraceId { get; set; } = default!;
    }

}
