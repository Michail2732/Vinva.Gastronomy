using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.CreateIngredient
{    
    public readonly record struct CreateIngredientCommand : IRequest<Result<CreateIngredientResponse>>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string? UsageComment { get; init; }        
    }
}
