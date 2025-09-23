using Card2CardGateway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.DTOs
{
    public class BankTransferResult
    {
        public TransactionStatus Status { get; set; }
        public string RequestTraceId { get; set; } = default!;
        public string? TransactionReferenceCode { get; set; }
        public string? ErrorCode { get; set; }
        public string? RawResponse { get; set; }
    }
    public class BankInquiryResult : BankTransferResult { }

}
