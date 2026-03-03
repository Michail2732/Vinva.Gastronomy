using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Entities
{
    public interface ISoftDeleteEntity : IEntity
    {
        bool IsDeleted { get; }
    }
}
