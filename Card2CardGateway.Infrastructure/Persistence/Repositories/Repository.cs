using Card2CardGateway.Application.UseCases.Transfer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context) => _context = context;

        public async Task AddAsync(T entity, CancellationToken ct = default) =>
            await _context.Set<T>().AddAsync(entity, ct);

        public async Task<T?> GetAsync(Guid id, CancellationToken ct = default) =>
            await _context.Set<T>().FindAsync(new object[] { id }, ct);

        public async Task<List<T>> GetAllAsync(CancellationToken ct = default) =>
            await _context.Set<T>().ToListAsync(ct);

        public async Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _context.Set<T>().Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(T entity, CancellationToken ct = default)
        {
            _context.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default) =>
            await _context.Set<T>().FindAsync(new object[] { id }, ct) is not null;
    }

}
