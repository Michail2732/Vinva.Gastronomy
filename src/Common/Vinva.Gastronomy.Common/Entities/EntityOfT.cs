using System;
using System.Collections.Generic;
using System.Text;
using UUIDNext;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Common
{
    public abstract class EntityOfT<T> : Entity, IEntityOfT<T>
        where T : struct
    {
        public T Id { get; protected set; }

        public EntityOfT(T id)
        {
            Id = id;
        }

        public EntityOfT()
        {
            if (TryGenerateId(out var newId))
                Id = newId;            
        }


        internal static bool TryGenerateId(out T field)
        {
            field = default;
            if (typeof(T) == typeof(Guid) && Uuid.NewSequential() is T newId)
            {
                field = newId;
                return true;
            }
            return false;
        }
        

        public override bool Equals(object? obj)
        {
            return Equals(obj as IEntityOfT<T>);
        }        

        public virtual bool Equals(IEntityOfT<T>? other)
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
