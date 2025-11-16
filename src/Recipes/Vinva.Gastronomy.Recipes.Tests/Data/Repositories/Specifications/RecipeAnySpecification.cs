using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Tests.Data.Repositories.Specifications
{
    public class RecipeAnySpecification: Specification<Recipe>
    {
        public RecipeAnySpecification()
        {
            Query.Where(a => true);
        }
    }
}
