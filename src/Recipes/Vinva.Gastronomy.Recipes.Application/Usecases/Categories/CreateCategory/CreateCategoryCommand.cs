using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.CreateCategory
{    
    public readonly record struct CreateCategoryCommand() : IRequest<CreateCategoryResponse>
    {
        public string Name { get; init; } = "";
        public string Description { get; init; } = "";
        public string? Comment { get; init; }
        public CategoryDtoType Type { get; init; }
    }
}