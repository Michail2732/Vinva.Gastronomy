using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Domain.Validations
{
    public class RecipeDomainErrors
    {
        public static string IngredientQuantitiesHasDuplicate(string ingredientQuantities) => $"Количество ингредиентов содержит дублирующиеся единицы измерения ингредиента: '{ingredientQuantities}'";
        public static string FailedParseIngredientQuantity(string str) => $"Не удалось преобразовать строку '{str}' в '{typeof(IngredientQuantity)}'";
        public static string FailedRemoveCategoryFromRecipe(Guid recipeId, Guid categoryId) => $"Не удалось удалить категорию '{categoryId}' из рецепта '{recipeId}'";
        public static string FailedRemoveIngredient(Guid recipeId, Guid ingredientId) => $"Не удалось удалить ингредиент '{ingredientId}' из рецепта '{recipeId}'";
        public static string FailedRemoveStep(Guid recipeId, Guid stepId) => $"Не удалось удалить шаг '{stepId}' из рецепта '{recipeId}'";
        public static string RecipeDoesNotContainsCategory(Guid recipeId, Guid categoryId) => $"Рецепт '{recipeId}' не содержит категорию '{categoryId}'";
        public static string IncorrectTypeOfRecipeCategory(Guid recipeId, Guid categoryId) => $"Некорректный тип категории '{categoryId}' для рецепта '{recipeId}'";
        public static string RecipeCategoryAlreadyExists(Guid recipeId, string name) => $"Рецепт '{recipeId}' уже содержит категорию '{name}'";
        public static string RecipeStepNotExists(Guid recipeId, Guid stepId) => $"Рецепт '{recipeId}' не содержит шага №'{stepId}'";
        public static string RecipeIngredientAlreadyExists(Guid recipeId, Guid ingredientId) => $"Рецепт '{recipeId}' уже содержит ингредиент '{ingredientId}'";
        public static string RecipeIngredientNotExists(Guid recipeId, Guid ingredientId) => $"Рецепт '{recipeId}' не содержит ингредиент '{ingredientId}'";        
        public static string IngredientCategoryAlreadyExists(Guid ingredientId, string name) => $"Ингредиент '{ingredientId}' уже содержит категорию '{name}'";
        public static string NotPossibleInheritNotBaseRecipe(Guid recipeId) => $"Невозможно сделать вариацию рецепта '{recipeId}' так как он не является базовым";


        public const string IncorrectIngredientMeasure = $"Некорректное значение меры ингредиента.";
        public const string IncorrectName = $"Некорректное значение наименования.";
        public const string IncorrectDescription = $"Некорректное значение описания.";
        public const string IncorrectComment = $"Некорректное значение комментария.";
        public const string IncorrectCookingTime = $"Некорректное значение времени приготовления.";
        public const string IncorrectIngredientQuantity = $"Некорректное значение количества ингредиента.";

    }
}
