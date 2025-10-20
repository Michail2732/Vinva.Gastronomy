using System;
using System.Collections.Generic;
using System.Text;
using UUIDNext;

namespace Vinva.Gastronomy.Common
{
    public abstract class EntityGuid: Entity<Guid>
    {
        public EntityGuid() 
        {
            Id = Uuid.NewSequential();
        }        
    }
}
