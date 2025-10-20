using System;
using System.Collections.Generic;
using System.Text;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    public class RecipeIngredient : Entity<RecipeIngredientId>
    {
        public string IngredientName { get; private set; }
        public string Measure { get; private set; }
        public bool IsRequired { get; private set; }
        public string? Comment { get; set; }


        public RecipeIngredient(RecipeIngredientId id, string ingredientName, string measure, bool isRequired)
        {
            Id = id;
            IngredientName = ingredientName ?? throw new ArgumentNullException(nameof(ingredientName));
            Measure = measure ?? throw new ArgumentNullException(nameof(measure));
            IsRequired = isRequired;
        }
    }
}
