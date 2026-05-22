using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Models;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Рецепт")]
    public class Recipe : DescriptiveSoftDeleteEntityOfT<Guid>, IAggregateRoot
    {        
        private readonly List<RecipeStep> _recipeSteps = new();
        private readonly List<RecipeIngredient> _recipeIngredients = new();
        private RecipeProperties _recipeProperties = RecipeProperties.CreatEmpty();


        public Guid? BaseRecipe { get; set; }        

        public Guid? TitleImageId { get; set; }
        
        public Guid? VideoId { get; set; }

        public TimeSpan? CookingTime { get; set; }

        public string? CookingComment { get; set; }        

        public string? IngredientComment { get; set; }

        public string? StorageComment { get; set; }

        public string? UsageComment { get; set; }

        public IReadOnlyList<RecipeStep> Steps => _recipeSteps;

        public IReadOnlyList<RecipeIngredient> Ingredients => _recipeIngredients;

        public List<Guid> OtherImageIds { get; init; } = new();

        public RecipeProperties Properties
        {
            get => _recipeProperties;
            set => _recipeProperties = value ?? throw new ArgumentNullException();
        }


#pragma warning disable CS8618
        private Recipe() { }
#pragma warning restore CS8618 

        public Recipe(string name, string description, Guid? baseRecipe = null) : base(name, description) 
        {
            BaseRecipe = baseRecipe;
        }

        public Recipe(Guid id, string name, string description, Guid? baseRecipe = null) : base(id, name, description)
        {
            BaseRecipe = baseRecipe;
        }                
        
        
        public Recipe AddStep(RecipeStep step)
        {
            var maxSeqNumber = _recipeSteps.Max(a => a.SeqNumber);            
            step.SeqNumber = maxSeqNumber+1;
            step.RecipeId = step.RecipeId;                        
            _recipeSteps.Add(step);
            return this; 
        }

        public void RemoveStep(Guid stepId)
        {
            var step = _recipeSteps.FirstOrDefault(a => a.Id == stepId);
            if (step == null)
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.RecipeStepNotExists(Id, stepId));
            if (!_recipeSteps.Remove(step))
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.FailedRemoveStep(Id, stepId));
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
