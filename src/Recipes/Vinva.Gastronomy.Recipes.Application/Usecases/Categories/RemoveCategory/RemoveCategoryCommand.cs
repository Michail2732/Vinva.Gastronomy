using MediatR;


namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.RemoveCategory
{    
    public readonly record struct RemoveCategoryCommand : IRequest
    {
        public Guid CategoryId { get; init; }
    }
}