using System;
using System.Threading;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        TRepo GetRepository<TRepo, TEntity>() 
            where TEntity : Entity
            where TRepo : ITransactionalRepository<TEntity>;
        bool IsTransactionOpen();
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitAsync(CancellationToken ct = default);
        Task RollbackAsync(CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task SaveAndCommitAsync(CancellationToken ct = default);
    }
}
