using Ardalis.Specification;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Tests.Data.Repositories.Specifications
{
    public class RecipeByIdSpecification : Specification<Recipe>
    {
        public RecipeByIdSpecification(Guid id)
        {
            Query.Where(a => a.Id == id);
        }
    }
}
