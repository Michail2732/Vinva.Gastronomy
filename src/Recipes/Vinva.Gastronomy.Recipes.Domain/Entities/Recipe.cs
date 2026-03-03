using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Полная информация о рецепте")]
    public class Recipe : DescriptiveSoftDeleteEntityOfT<Guid>, IAggregateRoot
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
        
        
        public Recipe AddStep(string description, string? comment, Guid? photoId = null)
        {
            var lastStep = _recipeSteps.LastOrDefault();
            
            var newStep = new RecipeStep(Id, description, photoId)
            {
                Comment = comment,
                SeqNumber = (lastStep?.SeqNumber ?? 0) + 1
            };

            _recipeSteps.Add(newStep);
            return this; 
        }                    

        public void ChangeStepOrder(int seqNumber1, int seqNumber2)
        {
            var step1 = _recipeSteps.FirstOrDefault(a => a.SeqNumber == seqNumber1);
            var step2 = _recipeSteps.FirstOrDefault(b => b.SeqNumber == seqNumber2);

            if (step1 == null)
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.RecipeStepNotExists(Id, seqNumber1));
            if (step2 == null)
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.RecipeStepNotExists(Id, seqNumber2));


            step1.SeqNumber = seqNumber2;
            step2.SeqNumber = seqNumber1;
        }

        public void RemoveStep(int seqNumber)
        {
            var step = _recipeSteps.FirstOrDefault(a => a.SeqNumber == seqNumber);
            if (step == null)
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.RecipeStepNotExists(Id, seqNumber));
            if (!_recipeSteps.Remove(step))
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.FailedRemoveStep(Id, seqNumber));
        }

        public void AddCategory(Category category)
        {                        
            if (category.Type != CategoryType.Recipe)
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.IncorrectTypeOfRecipeCategory(Id, category.Id));

            if (_recipeCategories.Contains(category))
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.RecipeCategoryAlreadyExists(Id, category.Name));

            _recipeCategories.Add(category);            
        }

        public void RemoveCategory(Guid categoryId)
        {
            var category = _recipeCategories.Find(a => a.Id == categoryId)
                ?? throw new RecipeDomainException(GetType(), RecipeDomainErrors.RecipeDoesNotContainsCategory(Id, categoryId));
            RemoveCategory(category);
        }

        public void RemoveCategory(Category category)
        {            
            if (!_recipeCategories.Remove(category))
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.FailedRemoveCategoryFromRecipe(Id, category.Id));
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
