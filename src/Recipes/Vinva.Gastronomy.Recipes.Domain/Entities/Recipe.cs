using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Models;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Рецепт")]
    public class Recipe : EntityOfT<Guid>, IAggregateRoot
    {                
        private readonly List<RecipeIngredient> _recipeIngredients = new();
        private RecipeProperties _recipeProperties = RecipeProperties.CreatEmpty();

        public string Name { get; }
        public string Description { get; }
        public Guid? BaseRecipe { get; set; }        
        public Guid? TitleImageId { get; set; }        
        public Guid? VideoId { get; set; }
        public TimeSpan? CookingTime { get; set; }
        public IReadOnlyList<RecipeIngredient> Ingredients => _recipeIngredients;
        public List<Guid> OtherImageIds { get; init; } = new();
        public string Document { get; set; }
        public RecipeProperties Properties
        {
            get => _recipeProperties;
            set => _recipeProperties = value ?? throw new ArgumentNullException();
        }


#pragma warning disable CS8618
        private Recipe() : base(GetDefaultGuid()) { }
#pragma warning restore CS8618 

        public Recipe(string name, string description, Guid? baseRecipe = null) : this(GenerateGuid(), name, description, baseRecipe)
        {
            
        }

        public Recipe(Guid id, string name, string description, Guid? baseRecipe = null) : base(id)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentException.ThrowIfNullOrEmpty(description);
            Name = name;
            Description = description;
            BaseRecipe = baseRecipe;            
        }                
        
                
        public void AddIngredient(RecipeIngredient ingredient)
        {
            if (_recipeIngredients.Contains(ingredient))
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.RecipeIngredientAlreadyExists(Id, ingredient.IngredientId));

            _recipeIngredients.Add(ingredient);
        }

        public void RemoveIngredient(Guid  ingredientId)
        {
            var recipeIngredient = Ingredients.FirstOrDefault(a => a.IngredientId == ingredientId);
            if (recipeIngredient == null)
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.RecipeIngredientNotExists(Id, ingredientId));
            if (!_recipeIngredients.Remove(recipeIngredient))
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.FailedRemoveIngredient(Id, ingredientId));
        }        
    }
}
