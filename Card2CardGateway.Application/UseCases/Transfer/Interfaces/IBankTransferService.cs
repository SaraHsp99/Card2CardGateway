using Card2CardGateway.Application.UseCases.Transfer.DTO;
using Card2CardGateway.Application.UseCases.Transfer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.Interfaces
{
    public interface IBankTransferService
    {
        string GetBankName();
        Task<BankTransferResult> TransferAsync(TransferRequestDto request, CancellationToken ct = default);
        Task<BankInquiryResult> InquiryAsync(string requestTraceId, CancellationToken ct = default);
    }

}
