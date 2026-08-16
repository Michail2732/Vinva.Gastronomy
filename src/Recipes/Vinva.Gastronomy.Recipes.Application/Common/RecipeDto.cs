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

        public Guid? BaseRecipe { get; init; }

        public Guid? TitleImageId { get; init; }

        public Guid? VideoId { get; init; }

        public TimeSpan? CookingTime { get; init; }        

        public IList<Guid> OtherImageIds { get; init; } = new List<Guid>();

        public RecipeIngredientDto[] Ingredients { get; init; } = [];

        public RecipePropertyDto[] Properties { get; init; } = [];

        public string? Document { get; init; }
    }
}
