using Card2CardGateway.Application.UseCases.Transfer.Interfaces;
using Card2CardGateway.Domain.Entities;
using Card2CardGateway.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Infrastructure.Persistence.Repositories
{
    
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context) : base(context) { }

        public async Task<Transaction?> GetByRequestTraceIdAsync(Guid requestTraceId, CancellationToken ct = default)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.RequestTraceId == requestTraceId, ct);
        }

        public async Task<IEnumerable<Transaction>> GetByStatusAsync(TransactionStatus status, CancellationToken ct = default)
        {
            return await _context.Transactions
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken ct = default)
        {
            return await _context.Transactions
                .Where(t => t.CreatedAt >= fromDate && t.CreatedAt <= toDate)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);
        }
    }

}
