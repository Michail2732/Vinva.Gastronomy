using System;

namespace Vinva.Gastronomy.Common.Entities
{
    public interface IEntityOfT<T> : IEntity
    {
        T Id { get; }
    }
}
