using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Recipes.Domain.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Models;
using Vinva.Gastronomy.Recipes.Domain.ValueObjects;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Рецепт")]
    public class Recipe : EntityGuid, IAggregateRoot
    {
        private readonly List<RecipeCategory> _recipeCategories = new();
        private readonly List<RecipeStep> _recipeSteps = new();
        private readonly List<RecipeIngredient> _recipeIngredients = new();
        private readonly List<Recipe> _variations = new();
        

        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid? PhotoId { get; set; }
        public Guid? VideoId { get; set; }        
        public TimeSpan? CookingTime { get; set; }
        public string? CookingComment { get; set; }
        public string? IngredientComment { get; set; }        
        public string? StorageComment { get; set; }
        public string? UsageComment { get; set; }
        public string? Comment { get; set; }

        public IReadOnlyList<RecipeCategory> Categories => _recipeCategories;
        public IReadOnlyList<RecipeStep> Steps => _recipeSteps;
        public IReadOnlyList<RecipeIngredient> Ingredients => _recipeIngredients;
        public IReadOnlyList<Recipe> Variations => _variations;


        public Recipe(string name, string description)
        {            
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));            
        }

        public Recipe(Guid id, string name, string description)
            : this(name, description) 
        {
            Id = id;
        }


        public Recipe SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(nameof(name));
            Name = name;
            return this;
        }

        public Recipe SetDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException(nameof(description));
            Description = Description;
            return this;
        }        
        
        public Recipe AddStep(string description, string comment, Guid? photoId = null)
        {
            var lastStep = _recipeSteps.LastOrDefault();
            int newSeqNumber = lastStep == null ? 1 : (lastStep.Id.SeqNumber + 1);
            _recipeSteps.Add(new RecipeStep(new RecipeStepId(Id, newSeqNumber), description, comment, photoId));
            return this; 
        }

        public RecipeCategory AddCategory(string name, string? comment = null)
        {
            var newId = new RecipeCategoryId(Id, name);
            if (_recipeCategories.Any(a => a.Id == newId))
                throw new RecipeDomainException(RecipeErrorMessages.RecipeCategoryAlreadyExists(Id, name));
            var newCategory = new RecipeCategory(newId, comment);
            _recipeCategories.Add(newCategory);
            return newCategory;
        }

        public RecipeIngredient AddRequiredIngredient(Guid ingredientId, string ingredientName, string measure, string? comment = null)
        {         
            return AddIngredientPrivate(ingredientId, ingredientName, measure, true, comment);
        }

        public RecipeIngredient AddIngredient(Guid ingredientId, string ingredientName, string measure, string? comment = null)
        {
            return AddIngredientPrivate(ingredientId, ingredientName, measure, false, comment);
        }


        private RecipeIngredient AddIngredientPrivate(Guid ingredientId, string ingredientName, string measure, bool isRequired, string? comment = null)
        {
            var newId = new RecipeIngredientId(Id, ingredientId);
            if (_recipeIngredients.Any(a => a.Id == newId))
                throw new RecipeDomainException(RecipeErrorMessages.RecipeIngredientAlreadyExists(Id, ingredientId));
            var newIngredient = new RecipeIngredient(newId, ingredientName, measure, isRequired)
            {
                Comment = comment
            };
            _recipeIngredients.Add(newIngredient);
            return newIngredient;
        }
    }
}
