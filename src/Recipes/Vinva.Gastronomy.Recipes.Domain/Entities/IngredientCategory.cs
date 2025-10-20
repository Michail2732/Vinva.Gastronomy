using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Domain.Entities
{
    [DisplayName("Категория ингредиента")]
    public class IngredientCategory: Entity<IngredientCategoryId>
    {
        public string? Comment { get; set; }

        public IngredientCategory(IngredientCategoryId id, string? comment = null)
        {
            Id = id;
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }
    }
}
