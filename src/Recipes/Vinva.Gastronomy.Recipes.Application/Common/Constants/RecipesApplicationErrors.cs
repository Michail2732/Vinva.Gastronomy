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
        /// <summary>
        /// Рецепт уже содержит категорию
        /// </summary>
        public static Error RecipeAlreadyContainsCategory(string categoryName) => new("Recipes.RecipeAlreadyContainsCategory", $"Рецепт уже содержит категорию '{categoryName}'");
        /// <summary>
        /// Не удалось найти категорию
        /// </summary>
        public static Error CategoryNotFound(Guid id) => new("Recipes.CategoryNotFound", $"Не удалось найти категорию '{id}'");
        /// <summary>
        /// Не удалось найти ингредиент по Id 
        /// </summary>
        public static Error IngredientNotFound(Guid id) => new("Recipes.IngredientNotFound", $"Не удалось найти ингредиент '{id}'");
    }
}
