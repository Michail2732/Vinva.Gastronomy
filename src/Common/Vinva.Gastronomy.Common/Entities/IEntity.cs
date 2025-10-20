using System;

namespace Vinva.Gastronomy.Common
{
    public interface IEntity<T>
        where T : struct
    {
        T Id { get; }
    }
}
