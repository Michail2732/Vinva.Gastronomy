using Vinva.Gastronomy.Recipes.Application.Common;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.Get
{
    public record SearchCategoriesQueryResponse 
    {
        public required IList<CategoryDto> Items { get; init; }      
    }
}
