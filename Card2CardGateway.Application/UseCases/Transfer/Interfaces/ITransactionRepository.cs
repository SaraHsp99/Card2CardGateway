using Card2CardGateway.Domain.Entities;
using Card2CardGateway.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<Transaction?> GetByRequestTraceIdAsync(string requestTraceId, CancellationToken ct = default);
        Task<IEnumerable<Transaction>> GetByStatusAsync(TransactionStatus status, CancellationToken ct = default);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken ct = default);
    }

}
