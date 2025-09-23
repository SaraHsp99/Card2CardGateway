using Card2CardGateway.Application.UseCases.Transfer.DTO;
using Card2CardGateway.Application.UseCases.Transfer.DTOs;
using Card2CardGateway.Application.UseCases.Transfer.Interfaces;
using Card2CardGateway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.Services
{
    public class SamanBankService : IBankTransferService
    {
        private readonly Random _random = new();
        public string GetBankName() => "Saman";

        public async Task<BankTransferResult> TransferAsync(TransferRequestDto request, CancellationToken ct = default)
        {

            await Task.Delay(500, ct);

            var mockStatuses = new[] { 0, 1, 2, 5 };
            var samanStatus = mockStatuses[_random.Next(mockStatuses.Length)];
            var standardizedStatus = ConvertSamanStatusToStandard(samanStatus);

            return new BankTransferResult
            {
                RequestTraceId = request.RequestTraceId,
                TransactionReferenceCode = Guid.NewGuid().ToString(),
                Status = standardizedStatus,
                ErrorCode = standardizedStatus == TransactionStatus.Success? null : $"BANK_ERR_{(int)standardizedStatus:000}",
                RawResponse = GetStatusMessage("Saman", samanStatus, standardizedStatus)
            };
        }

        public async Task<BankInquiryResult> InquiryAsync(string requestTraceId, CancellationToken ct = default)
        {
            await Task.Delay(300, ct);

            var mockStatuses = new[] { 0, 1, 2, 5 };
            var samanStatus = mockStatuses[_random.Next(mockStatuses.Length)];
            var standardizedStatus = ConvertSamanStatusToStandard(samanStatus);

            var rawResponse = GetStatusMessage("Saman", samanStatus, standardizedStatus);

            return new BankInquiryResult
            {
                RequestTraceId = requestTraceId,
                TransactionReferenceCode = Guid.NewGuid().ToString(),
                Status = standardizedStatus,
                ErrorCode = standardizedStatus == TransactionStatus.Success ? null : $"BANK_ERR_{(int)standardizedStatus:000}",
                RawResponse = rawResponse
            };
        }
        private string GetStatusMessage(string bank, int bankStatus, TransactionStatus standardizedStatus)
        {
            var message = standardizedStatus switch
            {
                TransactionStatus.Unknown => "خطای نامشخص",
                TransactionStatus.Success => "موفق",
                TransactionStatus.InsufficientFunds => "عدم موجودی",
                TransactionStatus.InvalidPin => "رمز اشتباه",
                TransactionStatus.InternalError => "خطای داخلی",
                _ => "وضعیت تعریف نشده"
            };

            return $"{{ \"Bank\":\"{bank}\", \"Status\":{bankStatus}, \"Message\":\"{message}\" }}";
        }

        private TransactionStatus ConvertSamanStatusToStandard(int samanStatus)
        {
            return samanStatus switch
            {
                0 => TransactionStatus.Success,
                1 => TransactionStatus.InsufficientFunds,
                2 => TransactionStatus.InvalidPin,
                5 => TransactionStatus.InternalError,
                _ => TransactionStatus.Unknown
            };
        }
    }
}
