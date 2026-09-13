using MediatR;
using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Create
{
    public sealed record CreateRecipeCommand : IRequest<CreateRecipeResponce>
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public string Document { get; init; } = string.Empty;
        public Guid? BaseRecipe { get; init; }
        public CharacteristicDto[] Characteristics { get; init; } = [];
    }
}
