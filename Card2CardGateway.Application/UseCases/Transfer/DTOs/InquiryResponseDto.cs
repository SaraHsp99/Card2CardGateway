using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Card2CardGateway.Application.UseCases.Transfer.DTOs
{
    public class InquiryResponseDto
    {
        public TransactionStatus Status { get; set; }
        public string RequestTraceId { get; set; } = default!;
        public string? TransactionReferenceCode { get; set; }
    }

}
