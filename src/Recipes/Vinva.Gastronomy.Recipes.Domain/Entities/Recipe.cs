using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Полная информация о рецепте")]
    public class Recipe : DescriptiveEntityOfT<Guid>, IAggregateRoot
    {
        private readonly List<Category> _recipeCategories = new();
        private readonly List<RecipeStep> _recipeSteps = new();
        private readonly List<RecipeIngredient> _recipeIngredients = new();
        private string? cookingComment;
        private string? ingredientComment;
        private string? storageComment;
        private string? usageComment;


        public Guid? BaseRecipe { get; private set; }

        public Guid? PhotoId { get; set; }

        public Guid? VideoId { get; set; }

        public TimeSpan? CookingTime { get; set; }

        public string? CookingComment
        {
            get => cookingComment;
            set => SetComment(value, ref cookingComment);
        }

        public string? IngredientComment
        {
            get => ingredientComment;
            set => SetComment(value, ref ingredientComment);
        }

        public string? StorageComment
        {
            get => storageComment;
            set => SetComment(value, ref storageComment);
        }

        public string? UsageComment
        {
            get => usageComment;
            set => SetComment(value, ref usageComment);
        }

        public IReadOnlyList<Category> Categories => _recipeCategories;

        public IReadOnlyList<RecipeStep> Steps => _recipeSteps;

        public IReadOnlyList<RecipeIngredient> Ingredients => _recipeIngredients;


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
        
        
        public Recipe AddStep(string name, string description, string comment, Guid? photoId = null)
        {
            var lastStep = _recipeSteps.LastOrDefault();
            int newSeqNumber = lastStep == null ? 1 : (lastStep.SeqNumber + 1);
            _recipeSteps.Add(new RecipeStep(Id, newSeqNumber, name, description, photoId)
            {
                Comment = comment,
            });
            return this; 
        }       

        public void AddCategory(Category category)
        {                        
            if (category.Type != CategoryType.Recipe)
                throw new RecipeDomainException(GetType(), RecipeErrorMessages.IncorrectTypeOfRecipeCategory(Id, category.Id));

            if (_recipeCategories.Contains(category))
                throw new RecipeDomainException(GetType(), RecipeErrorMessages.RecipeCategoryAlreadyExists(Id, category.Name));

            _recipeCategories.Add(category);            
        }

        public void RemoveCategory(Guid categoryId)
        {
            var category = _recipeCategories.Find(a => a.Id == categoryId)
                ?? throw new RecipeDomainException(GetType(), RecipeErrorMessages.RecipeDoesNotContainsCategory(Id, categoryId));
            RemoveCategory(category);
        }

        public void RemoveCategory(Category category)
        {            
            if (!_recipeCategories.Remove(category))
                throw new RecipeDomainException(GetType(), RecipeErrorMessages.FailedRemoveCategoryFromRecipe(Id, category.Id));
        }

        public RecipeIngredient AddRequiredIngredient(Guid ingredientId, string ingredientName, string description, string measure, string? comment = null)
        {         
            return AddIngredientPrivate(ingredientId, ingredientName, description, measure, true, comment);
        }

        public RecipeIngredient AddIngredient(Guid ingredientId, string ingredientName, string description, string measure, string? comment = null)
        {
            return AddIngredientPrivate(ingredientId, ingredientName, description, measure, false, comment);
        }


        private RecipeIngredient AddIngredientPrivate(Guid ingredientId, string ingredientName, string description, string measure, bool isRequired, string? comment = null)
        {                        
            var newIngredient = new RecipeIngredient(Id, ingredientId, ingredientName, description, measure, isRequired) 
            {
                Comment = comment 
            };

            if (_recipeIngredients.Contains(newIngredient))
                throw new RecipeDomainException(GetType(), RecipeErrorMessages.RecipeIngredientAlreadyExists(Id, ingredientId));

            _recipeIngredients.Add(newIngredient);
            return newIngredient;
        }        
    }
}
