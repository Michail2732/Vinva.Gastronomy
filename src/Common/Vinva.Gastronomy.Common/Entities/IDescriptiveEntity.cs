using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Common.Entities
{
    public interface IDescriptiveEntity : IEntity
    {                
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Comment { get; set; }        
    }
}
