using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Vinva.Gastronomy.Common.Repositories;

namespace Vinva.Gastronomy.Common.Infrastructure.EntityFramework
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        protected IDbContextTransaction? _transaction;
        protected readonly DbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories;


        protected UnitOfWorkBase(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _repositories = new Dictionary<Type, object>();
            InitRepositories(_repositories);
        }


        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext?.Dispose();
        }

        public TRepo GetRepository<TRepo, TEntity>()
            where TEntity : Entity
            where TRepo : ITransactionalRepository<TEntity>
        {
            return (TRepo)_repositories[typeof(TRepo)];
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(ct);
                await _transaction.DisposeAsync();
                _transaction = null;
            }                
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }                            
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task SaveAndCommitAsync(CancellationToken ct = default)
        {
            if (_transaction == null)
            {
                await BeginTransactionAsync(ct);
            }
            await SaveChangesAsync(ct);
            await CommitAsync(ct);
        }

        protected abstract void InitRepositories(Dictionary<Type, object> repoStorage);

        public bool IsTransactionOpen()
        {
            return _transaction != null;
        }
    }
}
