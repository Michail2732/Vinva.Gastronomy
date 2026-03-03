using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.RemoveCategory
{    
    public readonly record struct RemoveCategoryCommand : IRequest
    {
        public Guid CategoryId { get; init; }
    }
}