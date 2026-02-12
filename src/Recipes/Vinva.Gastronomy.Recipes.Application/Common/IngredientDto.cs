using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public record IngredientDto
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
        public string? UsageComment { get; init; }
        public Guid? PhotoId { get; init; }
        public Guid? RecipeId { get; init; }
        public IngredientCategoryDto[] Categories { get; init; } = [];
    }
}
