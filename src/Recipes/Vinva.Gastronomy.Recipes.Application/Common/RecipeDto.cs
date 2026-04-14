using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public record RecipeDto
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }        

        public string? Description { get; init; }

        public string? Comment { get; init; }

        public Guid? BaseRecipe { get; init; }

        public Guid? PhotoId { get; init; }

        public Guid? VideoId { get; init; }

        public TimeSpan? CookingTime { get; init; }

        public string? CookingComment { get; init; }

        public string? IngredientComment { get; init; }

        public string? StorageComment { get; init; }

        public string? UsageComment { get; init; }

        public RecipeIngredientDto[] Ingredients { get; init; } = [];

        public RecipeCategoryDto[] Categories { get; init; } = [];

        public RecipeStepDto[] Steps { get; init; } = [];
    }
}
