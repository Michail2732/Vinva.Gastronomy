using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Recipes.Domain.Constants;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Ингредиент")]
    public class Ingredient: EntityGuid, IAggregateRoot
    {
        private readonly List<IngredientCategory> _categories = new();


        public string Name { get; private set; }
        public string? Description { get; set; }
        public string? UsageComment { get; set; }
        public string? Comment { get; set; }
        public Guid? PhotoId { get; set; }
        public Guid? RecipeId { get; set; }
        public IReadOnlyList<IngredientCategory> Categories => _categories;


        public Ingredient(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));            
        }

        public Ingredient(Guid id, string name)
            :this(name)
        {
            Id = id;
        }


        public Ingredient SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(nameof(name));
            Name = name;
            return this;
        }        

        public Ingredient AddCategory(string category, string? comment  = null)
        {
            var newId = new IngredientCategoryId(Id, category);
            if (_categories.Any(a => a.Id == newId))
                throw new RecipeDomainException(RecipeErrorMessages.IngredientCategoryAlreadyExists(Id, category));
            _categories.Add(new IngredientCategory(newId, comment));
            return this;
        }
    }
}
