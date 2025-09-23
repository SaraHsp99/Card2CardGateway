using Card2CardGateway.Application.UseCases.Transfer.DTO;
using Card2CardGateway.Application.UseCases.Transfer.DTOs;
using Card2CardGateway.Application.UseCases.Transfer.Interfaces;
using Card2CardGateway.Application.UseCases.Transfer.Validators;
using Card2CardGateway.Domain.Entities;
using Card2CardGateway.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;

namespace Card2CardGateway.Application.UseCases.Transfer.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly IBankServiceFactory _bankFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionService> _logger;
        private readonly IValidator<TransferRequestDto> _validator;
        private readonly AsyncRetryPolicy _retryPolicy;

        public TransactionService(
            IBankServiceFactory bankFactory,
            IUnitOfWork unitOfWork,
            ILogger<TransactionService> logger,
            IValidator<TransferRequestDto> validator)
            
        {
            _bankFactory = bankFactory;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _validator = validator;


            _retryPolicy = Policy
                .Handle<BankTemporaryException>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * attempt),
                    (ex, ts, attempt, ctx) =>
                    {
                        _logger.LogWarning("Retry {Attempt} due to temporary bank error: {Message}", attempt, ex.Message);
                    });
        }

        public async Task<BankTransferResult> TransferAsync(TransferRequestDto request, CancellationToken ct = default)
        {
            _logger.LogInformation("Starting transfer for RequestTraceId: {RequestTraceId}", request.RequestTraceId);

            
            var validationResult = await _validator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

           
            if (await _unitOfWork.Transactions.ExistsAsync(request.RequestTraceId, ct))
                throw new InvalidOperationException($"Transaction with RequestTraceId {request.RequestTraceId} already exists");

            var bankService = _bankFactory.Resolve(request.SourceCardNumber);

            
            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                BankTransferResult bankResult = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var result = await bankService.TransferAsync(request, ct);

                    if (result.Status == TransactionStatus.InternalError)
                        throw new BankTemporaryException("Bank internal error");

                    return result;
                });

                var transaction = new Transaction
                {
                    RequestTraceId = request.RequestTraceId,
                    SourceCardNumber = request.SourceCardNumber,
                    DestinationCardNumber = request.DestinationCardNumber,
                    Amount = request.Amount,
                    Status = bankResult.Status,
                    TransactionReferenceCode = bankResult.TransactionReferenceCode,
                    CreatedAt = DateTime.UtcNow,
                    RawResponse = bankResult.RawResponse
                };

                await _unitOfWork.Transactions.AddAsync(transaction, ct);
                await _unitOfWork.SaveChangesAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);

                _logger.LogInformation("Transfer completed successfully for RequestTraceId: {RequestTraceId}", request.RequestTraceId);
                return bankResult;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                _logger.LogError(ex, "Transfer failed for RequestTraceId: {RequestTraceId}", request.RequestTraceId);
                throw;
            }
        }

        public async Task<BankInquiryResult> InquiryAsync(Guid requestTraceId, CancellationToken ct = default)
        {
            _logger.LogInformation("Starting inquiry for RequestTraceId: {RequestTraceId}", requestTraceId);

            var transaction = await _unitOfWork.Transactions.GetByRequestTraceIdAsync(requestTraceId, ct);
            if (transaction == null)
            {
                _logger.LogWarning("Transaction not found for RequestTraceId: {RequestTraceId}", requestTraceId);
                throw new KeyNotFoundException($"Transaction with RequestTraceId {requestTraceId} not found");
            }

            var bankService = _bankFactory.Resolve(transaction.SourceCardNumber);

            var inquiryResult = await bankService.InquiryAsync(requestTraceId, ct);

            if (transaction.Status != inquiryResult.Status)
            {
                transaction.Status = inquiryResult.Status;
                transaction.TransactionReferenceCode = inquiryResult.TransactionReferenceCode;
                transaction.RawResponse = inquiryResult.RawResponse;
                transaction.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Transactions.UpdateAsync(transaction, ct);
                await _unitOfWork.SaveChangesAsync(ct);

                _logger.LogInformation("Transaction status updated for RequestTraceId: {RequestTraceId}", requestTraceId);
            }

            return inquiryResult;
        }

       
    }


    public class BankTemporaryException : Exception
    {
        public BankTemporaryException(string message) : base(message) { }
    }


}
