using Card2CardGateway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Domain.Entities
{
    public class Transaction
    {
            public Guid Id { get; set; } = Guid.NewGuid();
            public Guid RequestTraceId { get; set; } = default!;
            public string SourceCardNumber { get; set; } = default!;
            public string DestinationCardNumber { get; set; } = default!;
            public decimal Amount { get; set; }          
            public TransactionStatus Status { get; set; }
            public string? TransactionReferenceCode { get; set; }
            public string BankName { get; set; } = default!;
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
            public string? ErrorCode { get; set; }
            public string? RawRequest { get; set; }
            public string? RawResponse { get; set; }
        
    }

}
