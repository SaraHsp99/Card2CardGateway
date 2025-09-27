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
    public class PasargadBankService : IBankTransferService
    {
        private readonly Random _random = new();
        public string GetBankName() => "Pasargad";

        public async Task<BankTransferResult> TransferAsync(TransferRequestDto request, CancellationToken ct = default)
        {
            await Task.Delay(500, ct);

            var bankStatuses = new[] { 20, 50, 60, 70 };
            var pasargadStatus = bankStatuses[_random.Next(bankStatuses.Length)];
            var standardizedStatus = ConvertPasargadStatusToStandard(pasargadStatus);

            var rawResponse = GetStatusMessage("Pasargad", pasargadStatus, standardizedStatus);

            return new BankTransferResult
            {
                RequestTraceId = request.RequestTraceId,
                TransactionReferenceCode = Guid.NewGuid().ToString(),
                Status = standardizedStatus,
                ErrorCode = standardizedStatus == TransactionStatus.Success ? null : $"BANK_ERR_{pasargadStatus}",
                RawResponse = rawResponse
            };
        }

        public async Task<BankInquiryResult> InquiryAsync(Guid requestTraceId, CancellationToken ct = default)
        {
            await Task.Delay(300, ct);

            
            var bankStatuses = new[] { 20, 50, 60, 70 };
            var pasargadStatus = bankStatuses[_random.Next(bankStatuses.Length)];
            var standardizedStatus = ConvertPasargadStatusToStandard(pasargadStatus);

           
            var rawResponse = GetStatusMessage("Pasargad", pasargadStatus, standardizedStatus);

            return new BankInquiryResult
            {
                RequestTraceId = requestTraceId,
                TransactionReferenceCode = Guid.NewGuid().ToString(),
                Status = standardizedStatus,
                ErrorCode = standardizedStatus == TransactionStatus.Success ? null : $"BANK_ERR_{pasargadStatus}",
                RawResponse = rawResponse
            };
        }


        private TransactionStatus ConvertPasargadStatusToStandard(int pasargadStatus)
        {
            return pasargadStatus switch
            {
                20 => TransactionStatus.Success,
                50 => TransactionStatus.InsufficientFunds,
                60 => TransactionStatus.InvalidPin,
                70 => TransactionStatus.InternalError,
                _ => TransactionStatus.Unknown
            };
        }

        private string GetStatusMessage(string bank, int bankStatus, TransactionStatus standardizedStatus)
        {
            var message = standardizedStatus switch
            {
                TransactionStatus.Success => "موفق",
                TransactionStatus.InsufficientFunds => "عدم موجودی",
                TransactionStatus.InvalidPin => "رمز اشتباه",
                TransactionStatus.InternalError => "خطای داخلی",
                TransactionStatus.Unknown or _ => "خطای نامشخص"
            };

            return System.Text.Json.JsonSerializer.Serialize(new
            {
                Bank = bank,
                Status = bankStatus,
                Message = message
            });
        }
    }
}


