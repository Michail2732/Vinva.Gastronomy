using System;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Common
{
    public abstract class Entity : IEntity
    {
        public abstract override bool Equals(object? obj);
        public abstract override int GetHashCode();
        public abstract bool Equals(IEntity? other);
    }
}
