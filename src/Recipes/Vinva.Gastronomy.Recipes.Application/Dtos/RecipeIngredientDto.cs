using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public sealed record QuantityDto
    {
        public required decimal Amount { get; init; }
        public required string Unit { get; init; }
    }

    public sealed record RecipeIngredientDto
    {
        public Guid? Id { get; init; }
        public required string Name { get; init; }
        public QuantityDto[] Quantities { get; init; } = [];
    }

    public sealed record RecipeIngredientsSetDto
    {
        public Guid? Id { get; init; }
        public required string Name { get; init; }
        public QuantityDto[] PortionsQuantities { get; init; } = [];
        public PortionsCalculationType PortionsCalculateType { get; init; }
        public RecipeIngredientDto[] Ingredients { get; init; } = [];
    }
}
