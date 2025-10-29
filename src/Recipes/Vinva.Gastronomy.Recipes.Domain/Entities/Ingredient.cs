using System;
using System.Collections.Generic;
using System.ComponentModel;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Ингредиент")]
    public class Ingredient: DescriptiveEntityOfT<Guid>, IAggregateRoot
    {
        private readonly List<IngredientCategory> _categories = new();
        
        public string? UsageComment { get; set; }        
        public Guid? PhotoId { get; set; }
        public Guid? RecipeId { get; set; }
        public IReadOnlyList<IngredientCategory> Categories => _categories;


#pragma warning disable CS8618
        private Ingredient() { }
#pragma warning restore CS8618 

        public Ingredient(string name, string description) : base(name, description) { }        

        public Ingredient(Guid id, string name, string description) : base(id, name, description) { }        
                            

        public IngredientCategory AddCategory(string category, string description, string? comment  = null)
        {                        
            var newCategory = new IngredientCategory(Id, category, description)
            {
                Comment = comment
            };

            if (_categories.Contains(newCategory))
                throw new RecipeDomainException(GetType(), RecipeErrorMessages.IngredientCategoryAlreadyExists(Id, category));

            _categories.Add(newCategory);
            return newCategory;
        }
    }
}
