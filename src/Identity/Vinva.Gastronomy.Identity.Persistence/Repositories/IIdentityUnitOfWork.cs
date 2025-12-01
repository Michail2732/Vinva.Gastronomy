using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Repositories;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Persistence.Repositories
{
    public interface IIdentityUnitOfWork : IUnitOfWork
    {
        ITransactionalRepository<User> Users { get; }
        ITransactionalRepository<UserTokens> UserTokens { get; }
    }
}
