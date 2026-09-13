using System;
using System.Collections.Generic;
using System.Text;
using UUIDNext;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Services;

namespace Vinva.Gastronomy.Common
{
    public abstract class EntityOfT<T> : Entity, IEntityOfT<T>        
    {
        public T Id { get; protected set; }

        public EntityOfT(T id)
        {
            Id = id;
        }        


        protected static Guid GenerateGuid()
        {
            return GuidProvider.Instance.Generate();            
        }

        protected static Guid GetDefaultGuid()
        {
            return GuidProvider.Instance.Generate();
        }

        protected static bool TryGenerateId(out T id)
        {
            if (typeof(T) == typeof(Guid))
            {
                id = (T)(object)GuidProvider.Instance.Generate();
                return true;
            }

            if (typeof(T) == typeof(string))
            {
                id = (T)(object)GuidProvider.Instance.Generate().ToString();
                return true;
            }

            id = default!;
            return false;
        }

        public override bool Equals(IEntity? other)
        {
            return Equals(other as IEntityOfT<T>);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as IEntityOfT<T>);
        }        

        public virtual bool Equals(IEntityOfT<T>? other)
        {
            return other is not null &&
                   EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
