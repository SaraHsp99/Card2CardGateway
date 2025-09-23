using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Card2CardGateway.Application.UseCases.Transfer.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken ct = default);
        Task<T?> GetAsync(Guid id, CancellationToken ct = default);
        Task<List<T>> GetAllAsync(CancellationToken ct = default);
        Task UpdateAsync(T entity, CancellationToken ct = default);
        Task DeleteAsync(T entity, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    }
}
