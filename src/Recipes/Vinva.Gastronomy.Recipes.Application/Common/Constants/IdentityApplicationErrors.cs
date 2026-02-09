using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.Constants
{
    public class RecipesApplicationErrors
    {        
        /// <summary>
        /// Не удалось найти ингредиент по Id 
        /// </summary>
        public static Error IngredientNotFound => new("Recipes.IngredientNotFound", "Ингредиент не удалось найти");

    }
}
