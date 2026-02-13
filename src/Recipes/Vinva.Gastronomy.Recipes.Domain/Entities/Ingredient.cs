using System;
using System.Collections.Generic;
using System.ComponentModel;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Models;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Ингредиент")]
    public class Ingredient: DescriptiveEntityOfT<Guid>, IAggregateRoot
    {
        private readonly List<Category> _categories = new();
        
        public string? UsageComment { get; set; }        
        public Guid? PhotoId { get; set; }
        public Guid? RecipeId { get; set; }
        public IReadOnlyList<Category> Categories => _categories;


#pragma warning disable CS8618
        private Ingredient() { }
#pragma warning restore CS8618 

        public Ingredient(string name, string description) : base(name, description) { }        

        public Ingredient(Guid id, string name, string description) : base(id, name, description) { }        
                            

        public Category AddCategory(string category, string description, string? comment  = null)
        {                        
            var newCategory = new Category(Id, category, description, CategoryType.Ingredient)
            {
                Comment = comment
            };

            if (_categories.Contains(newCategory))
                throw new RecipeDomainException(GetType(), RecipeDomainErrors.IngredientCategoryAlreadyExists(Id, category));

            _categories.Add(newCategory);
            return newCategory;
        }

        public RecipeIngredient ToRecipeIngredient(Guid recipeId, IngredientQuantities quantities, bool isRequired = false)
        {
            return new RecipeIngredient(recipeId, Id, Name, Description, quantities, isRequired);
        }
    }
}
