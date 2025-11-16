using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Tests.Data.Repositories.Specifications
{
    public class RecipeInfoByIdSpecification : Specification<RecipeInfo>
    {
        public RecipeInfoByIdSpecification(Guid id)
        {
            Query.Where(a => a.Id == id);
        }
    }
}
