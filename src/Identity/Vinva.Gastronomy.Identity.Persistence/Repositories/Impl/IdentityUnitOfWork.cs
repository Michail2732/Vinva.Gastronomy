using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.EntityFramework;
using Vinva.Gastronomy.Common.Repositories;
using Vinva.Gastronomy.Identity.Domain.Entities;

namespace Vinva.Gastronomy.Identity.Persistence.Repositories
{
    public class IdentityUnitOfWork : UnitOfWorkBase, IIdentityUnitOfWork
    {
        public ITransactionalRepository<User> Users { get; private set; }

        public ITransactionalRepository<UserTokens> UserTokens { get; private set; }

        public IdentityUnitOfWork(IdentityDbContext dbContext) : base(dbContext)
        {
            Users = null!;
            UserTokens = null!;
        }        

        protected override void InitRepositories(Dictionary<Type, object> repoStorage)
        {            
            repoStorage[typeof(ITransactionalRepository<User>)] = Users = new TransactionalRepository<User>(_dbContext);
            repoStorage[typeof(ITransactionalRepository<UserTokens>)] = UserTokens = new TransactionalRepository<UserTokens>(_dbContext);
        }
    }
}
