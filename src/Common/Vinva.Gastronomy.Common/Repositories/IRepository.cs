using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vinva.Gastronomy.Common.Repositories
{
    public interface IRepository<TEntity>: IRepositoryBase<TEntity>
        where TEntity: Entity
    {
    }
}
