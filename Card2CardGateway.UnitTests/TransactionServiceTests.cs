using Card2CardGateway.Application.UseCases.Transfer.DTO;
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
    public class TransactionServiceTests
    {
        [Fact]
        public async Task TransferAsync_ShouldThrow_WhenDuplicateTransaction()
        {
           
            var mockFactory = new Mock<IBankServiceFactory>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBankService = new Mock<IBankTransferService>();

            var validator = new TransferRequestValidator(
                Microsoft.Extensions.Options.Options.Create(new TransferOptions { MinAmount = 1000, MaxAmount = 10000000 })
            );

            mockUnitOfWork.Setup(u => u.Transactions.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var service = new TransactionService(
                mockFactory.Object,
                mockUnitOfWork.Object,
                NullLogger<TransactionService>.Instance,
                validator
            );

            var request = new TransferRequestDto
            {
                RequestTraceId = Guid.NewGuid(),
                Amount = 5000,
                SourceCardNumber = "1234567812345678",
                DestinationCardNumber = "8765432187654321",
                Cvv2 = "123",
                Pin2 = "1234",
                ExpiredDate = "2705"
            };

            // Act + Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.TransferAsync(request, CancellationToken.None));
        }

        [Fact]
        public async Task TransferAsync_ShouldSucceed_WhenNewTransaction()
        {
            
            var mockFactory = new Mock<IBankServiceFactory>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockBankService = new Mock<IBankTransferService>();

            var validator = new TransferRequestValidator(
                Microsoft.Extensions.Options.Options.Create(new TransferOptions { MinAmount = 1000, MaxAmount = 10000000 })
            );

            mockUnitOfWork.Setup(u => u.Transactions.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false); 

            mockBankService.Setup(b => b.TransferAsync(It.IsAny<TransferRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BankTransferResult
                {
                    Status = TransactionStatus.Success,
                    RequestTraceId = Guid.NewGuid(),
                    TransactionReferenceCode = "ABC123",
                    RawResponse = "OK"
                });

            mockFactory.Setup(f => f.Resolve(It.IsAny<string>()))
                .Returns(mockBankService.Object);

            var service = new TransactionService(
                mockFactory.Object,
                mockUnitOfWork.Object,
                NullLogger<TransactionService>.Instance,
                validator
            );

            var request = new TransferRequestDto
            {
                RequestTraceId = Guid.NewGuid(),
                Amount = 5000,
                SourceCardNumber = "1234567812345678",
                DestinationCardNumber = "8765432187654321",
                Cvv2 = "123",
                Pin2 = "1234",
                ExpiredDate = "2705"
            };

     
            var result = await service.TransferAsync(request, CancellationToken.None);

 
            Assert.Equal(TransactionStatus.Success, result.Status);
            Assert.Equal("ABC123", result.TransactionReferenceCode);
            Assert.Equal("OK", result.RawResponse);
        }
    }
}
