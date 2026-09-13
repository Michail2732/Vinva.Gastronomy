using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Recipes.Application.Constants
{
    public class RecipesApplicationErrors
    {                
        /// <summary>
        /// Не удалось найти рецепт по Id 
        /// </summary>
        public static Error RecipeNotFound => new("Recipes.RecipeNotFound", "Рецепт не удалось найти");
    }
}
