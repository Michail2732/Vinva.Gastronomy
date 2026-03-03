using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Entities
{
    public class DescriptiveSoftDeleteEntityOfT<T> : DescriptiveEntityOfT<T>, ISoftDeleteEntity
        where T : struct
    {
        public bool IsDeleted { get; private set; }


#pragma warning disable CS8618
        protected DescriptiveSoftDeleteEntityOfT() { }
#pragma warning restore CS8618 

        protected DescriptiveSoftDeleteEntityOfT(T id, string name, string description) : base(name, description)
        {
            Id = id;
        }

        protected DescriptiveSoftDeleteEntityOfT(string name, string description) : base(name, description)
        {
            if (EntityOfT<T>.TryGenerateId(out var newId))
                Id = newId;
        }


        public virtual void Delete()
        {
            IsDeleted = true;
        }

        public virtual void Restore()
        {
            IsDeleted = false;
        }
    }
}
