using System;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        Task<int> SaveChangesAsync();
        Task SaveAndCommitAsync();
    }
}
