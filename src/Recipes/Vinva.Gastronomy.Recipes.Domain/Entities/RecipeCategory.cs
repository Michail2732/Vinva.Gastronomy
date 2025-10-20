using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Recipes.Domain.ValueObjects;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Категория рецепта")]
    public class RecipeCategory : Entity<RecipeCategoryId>
    {
        public string Comment { get; private set; }

        public RecipeCategory(RecipeCategoryId id, string? comment = null)
        {
            Id = id;
            Comment = comment ?? "";
        }        
    }
}
