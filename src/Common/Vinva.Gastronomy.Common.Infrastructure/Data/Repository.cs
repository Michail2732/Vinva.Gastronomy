using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Repositories;

namespace Vinva.Gastronomy.Common.Infrastructure.EntityFramework
{
    public class Repository<C, T> : RepositoryBase<T>, IRepository<T>
        where C : DbContext
        where T : Entity
    {        
        public Repository(C dbContext) : base(dbContext)
        {
            
        }

        protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
        {
            if (specification.Selector is not null)
            {
                return base.ApplySpecification(specification);
            }

            if (typeof(TResult).IsAssignableFrom(typeof(T)))
            {
                return (IQueryable<TResult>)ApplySpecification(specification, false);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
