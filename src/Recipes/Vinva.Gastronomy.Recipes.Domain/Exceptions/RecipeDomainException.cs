using System;
using System.Collections.Generic;
using System.Text;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Domain.Exceptions
{
	[Serializable]
	public class RecipeDomainException : DomainException
	{		
		public RecipeDomainException(Type type, string message, Exception? inner = null) : base(type, message, inner) { }
		public RecipeDomainException(string message, Exception? inner = null) : base(message, inner) { }        

        public static RecipeDomainException RecipeCharacteristicAlreadyExists(Guid recipeId, string characteristicKey) => 
            new RecipeDomainException(typeof(Recipe), $"Рецепт '{recipeId}' уже содержит свойство'{characteristicKey}'");

        public static RecipeDomainException RecipeCharacteristicNotExists(Guid recipeId, string characteristicName) =>
            new RecipeDomainException(typeof(Recipe), $"Рецепт '{recipeId}' не содержит свойство '{characteristicName}'");

        public static RecipeDomainException RecipeIngredientAlreadyExists(Guid recipeId, string ingredientName) =>
            new RecipeDomainException(typeof(Recipe), $"Рецепт '{recipeId}' уже содержит ингредиент '{ingredientName}'");

        public static RecipeDomainException RecipeIngredientNotExists(Guid recipeId, string ingredientName) =>
            new RecipeDomainException(typeof(Recipe), $"Рецепт '{recipeId}' не содержит ингредиент '{ingredientName}'");


        public static RecipeDomainException IncorrectCharacteristicMeasure(Guid recipeId, string characteristicName) =>
            new RecipeDomainException(typeof(RecipeCharacteristic), $"Некорректное значение меры '{characteristicName}' свойства в рецепте '{recipeId}'.");
        public static RecipeDomainException IncorrectName() =>
            new RecipeDomainException($"Некорректное значение наименования.");
        public static RecipeDomainException IncorrectDescription() =>
            new RecipeDomainException($"Некорректное значение описания.");
    }
}
