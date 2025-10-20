using System;
using System.Collections.Generic;
using System.Text;

namespace Vinva.Gastronomy.Common
{
    public abstract class Entity<T> : IEntity<T>, IEquatable<Entity<T>?>
        where T : struct
    {
        public T Id { get; protected set; }


        public override bool Equals(object? obj)
        {
            return Equals(obj as EntityGuid);
        }

        public virtual bool Equals(Entity<T>? other)
        {
            return other is not null &&
                   Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
