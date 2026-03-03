using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{

    public class Category : DescriptiveSoftDeleteEntityOfT<Guid>
    {
        public CategoryType Type { get; private set; }


#pragma warning disable CS8618
        private Category() { }
#pragma warning restore CS8618 

        public Category(Guid id, string name, string description, CategoryType type) : base(id, name, description)
        {            
            Type = type;
        }

        public Category(string name, string description, CategoryType type) : base(name, description)
        {
            Type = type;
        }

    }
}
