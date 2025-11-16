using Ardalis.Specification;

namespace Vinva.Gastronomy.Common.Repositories
{
    public interface IReadRepository<TEntity> : IReadRepositoryBase<TEntity>
        where TEntity : Entity
    {
    }
}
