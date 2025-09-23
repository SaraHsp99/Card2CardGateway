using Card2CardGateway.Application.UseCases.Transfer.DTO;
using Card2CardGateway.Application.UseCases.Transfer.DTOs;
using Card2CardGateway.Domain.Entities;
using Card2CardGateway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.Interfaces
{
    public interface ITransactionService
    {
        Task<BankTransferResult> TransferAsync(TransferRequestDto request, CancellationToken ct = default);
        Task<BankInquiryResult> InquiryAsync(Guid requestTraceId, CancellationToken ct = default);
        //Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(
        //    DateTime? fromDate = null,
        //    DateTime? toDate = null,
        //    TransactionStatus? status = null,
        //    CancellationToken ct = default);
    }

}
