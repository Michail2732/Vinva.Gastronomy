using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.CategoryUsecases.CreateCategory
{
    public readonly record struct CreateCategoryResponse 
    {
        public Guid CategoryId { get; init; }        
    }
}
