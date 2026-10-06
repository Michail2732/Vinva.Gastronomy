namespace Vinva.Gastronomy.Recipes.Application.Common
{
    public sealed record RecipeIngredientQuantityDto
    {
        public required decimal Amount { get; init; }
        public required string Unit { get; init; }
    }

    public sealed record RecipeIngredientDto
    {
        public required string Name { get; init; }
        public string? Part { get; init; }
        public RecipeIngredientQuantityDto[] Quantities { get; init; } = [];
    }
}
