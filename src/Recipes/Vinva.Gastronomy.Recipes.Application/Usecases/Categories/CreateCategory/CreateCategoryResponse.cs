using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.CreateCategory
{
    public readonly record struct CreateCategoryResponse 
    {
        public Guid CategoryId { get; init; }        
    }
}
