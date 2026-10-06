using MediatR;
using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Update
{
    public sealed record UpdateCommand : IRequest
    {
        public required Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? Document { get; init; }
        public Guid? BaseRecipe { get; init; }
        public CharacteristicDto[]? Characteristics { get; init; }
        public RecipeIngredientDto[]? Ingredients { get; init; }
    }
}
