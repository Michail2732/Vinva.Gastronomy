using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Exceptions
{

	[Serializable]
	public class EntityDomainException : DomainException
    {
        public Type EntityType { get; private set; }

        public EntityDomainException(Type entityType)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
        }
        public EntityDomainException(Type entityType, string message) : base(message)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
        }
        public EntityDomainException(Type entityType, string message, Exception inner) : base(message, inner)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
        }
    }
}
