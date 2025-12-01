using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Repositories;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Persistence.Repositories
{
    public interface IRecipeStepsRepository : IRepository<RecipeStep>
    {
    }
}
