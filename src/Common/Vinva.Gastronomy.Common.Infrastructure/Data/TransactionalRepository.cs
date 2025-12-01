using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Repositories;

namespace Vinva.Gastronomy.Common.Infrastructure.EntityFramework
{
    public class TransactionalRepository<TEntity> : RepositoryBase<TEntity>, ITransactionalRepository<TEntity>
        where TEntity : Entity
    {
        public TransactionalRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<int>(default);
        }

        async Task<int> ITransactionalRepository<TEntity>.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
