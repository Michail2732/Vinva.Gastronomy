using System;

namespace Vinva.Gastronomy.Common
{
    public interface IEntity { }

    public interface IEntity<T> : IEntity
        where T : struct
    {
        T Id { get; }
    }
}
