using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public readonly record struct IngredientDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string? UsageComment { get; init; }
        public Guid? PhotoId { get; init; }
        public Guid? RecipeId { get; init; }
        public IngredientCategoryDto[] Categories { get; init; }
    }
}
