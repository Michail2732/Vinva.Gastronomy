using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Common.Repositories
{
    /// <summary>
    /// Облегчённый репозиторий для работы с транзакциями 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ITransactionalRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {

        /// <summary>
        /// Persists changes to the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
