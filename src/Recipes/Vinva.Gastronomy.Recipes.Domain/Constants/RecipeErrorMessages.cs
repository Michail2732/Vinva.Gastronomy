using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Constants
{
    public class RecipeErrorMessages
    {
        public static string RecipeCategoryAlreadyExists(Guid recipeId, string name) => $"Рецепт '{recipeId}' уже содержит категорию '{name}'";
        public static string RecipeIngredientAlreadyExists(Guid recipeId, Guid ingredientId) => $"Рецепт '{recipeId}' уже содержит ингредиент '{ingredientId}'";
        public static string IngredientCategoryAlreadyExists(Guid ingredientId, string name) => $"Ингредиент '{ingredientId}' уже содержит категорию '{name}'";
        public static string NotPossibleInheritNotBaseRecipe(Guid recipeId) => $"Невозможно сделать вариацию рецепта '{recipeId}' так как он не является базовым";        
    }
}
