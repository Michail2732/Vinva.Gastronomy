using MediatR;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Remove
{
    public sealed record RemoveCommand : IRequest
    {
        public required Guid RecipeId { get; init; }
    }
}
