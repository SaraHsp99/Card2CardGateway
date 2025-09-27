using Card2CardGateway.Application.UseCases.Transfer.DTOs;
using Card2CardGateway.Application.UseCases.Transfer.Interfaces;
using Card2CardGateway.Application.UseCases.Transfer.Options;
using Card2CardGateway.Application.UseCases.Transfer.Services;
using Card2CardGateway.Application.UseCases.Transfer.Validators;
using Card2CardGateway.Domain.Entities;
using Card2CardGateway.Domain.Enums;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Card2CardGateway.UnitTests
{
    public class TransactionServiceInquiryTests
    {
        [Fact]
        public async Task InquiryAsync_ShouldThrow_WhenTransactionNotFound()
        {
            
            var mockFactory = new Mock<IBankServiceFactory>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var validator = new TransferRequestValidator(
                Microsoft.Extensions.Options.Options.Create(new TransferOptions { MinAmount = 1000, MaxAmount = 10000000 })
            );

            mockUnitOfWork.Setup(u => u.Transactions.GetByRequestTraceIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Transaction?)null);

            var service = new TransactionService(
                mockFactory.Object,
                mockUnitOfWork.Object,
                NullLogger<TransactionService>.Instance,
                validator
            );

            var requestTraceId = Guid.NewGuid();

        
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                service.InquiryAsync(requestTraceId, CancellationToken.None));
        }

        [Fact]
        public async Task InquiryAsync_ShouldReturnResult_AndUpdateStatus_WhenTransactionFound()
        {
            
            var mockFactory = new Mock<IBankServiceFactory>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBankService = new Mock<IBankTransferService>();

            var validator = new TransferRequestValidator(
                Microsoft.Extensions.Options.Options.Create(new TransferOptions { MinAmount = 1000, MaxAmount = 10000000 })
            );

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                RequestTraceId = Guid.NewGuid(),
                SourceCardNumber = "1234567812345678",
                DestinationCardNumber = "8765432187654321",
                Amount = 5000,
                Status = TransactionStatus.InsufficientFunds,
                CreatedAt = DateTime.UtcNow
            };

            mockUnitOfWork.Setup(u => u.Transactions.GetByRequestTraceIdAsync(transaction.RequestTraceId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction);

            mockBankService.Setup(b => b.InquiryAsync(transaction.RequestTraceId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BankInquiryResult
                {
                    Status = TransactionStatus.Success,
                    RequestTraceId = transaction.RequestTraceId,
                    TransactionReferenceCode = "XYZ789",
                    RawResponse = "INQUIRY_OK"
                });

            mockFactory.Setup(f => f.Resolve(transaction.SourceCardNumber))
                .Returns(mockBankService.Object);

            var service = new TransactionService(
                mockFactory.Object,
                mockUnitOfWork.Object,
                NullLogger<TransactionService>.Instance,
                validator
            );

            
            var result = await service.InquiryAsync(transaction.RequestTraceId, CancellationToken.None);

          
            Assert.Equal(TransactionStatus.Success, result.Status);
            Assert.Equal("XYZ789", result.TransactionReferenceCode);
            Assert.Equal("INQUIRY_OK", result.RawResponse);

          
            mockUnitOfWork.Verify(u => u.Transactions.UpdateAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
