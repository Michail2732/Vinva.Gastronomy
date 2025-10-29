using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Common.Entities
{
    public class DescriptiveEntityOfT<T> : DescriptiveEntity, IEntityOfT<T>
        where T : struct
    {
        public T Id { get; protected set; }

#pragma warning disable CS8618
        protected DescriptiveEntityOfT() { }
#pragma warning restore CS8618 

        protected DescriptiveEntityOfT(T id, string name, string description) : base(name, description)
        {
            Id = id;
        }

        protected DescriptiveEntityOfT(string name, string description) : base(name, description)
        {
            if (EntityOfT<T>.TryGenerateId(out var newId))
                Id = newId;
        }


        public override bool Equals(object? obj)
        {
            return Equals(obj as IEntityOfT<T>);
        }

        public override bool Equals(IEntity? other)
        {
            return other is not null &&
                   other is IEntityOfT<T> castEntity &&
                   Id.Equals(castEntity.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }        
    }
}
