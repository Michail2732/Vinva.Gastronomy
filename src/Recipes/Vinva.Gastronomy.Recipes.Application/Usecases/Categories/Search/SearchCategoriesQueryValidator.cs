using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Categories.Get
{
    public class SearchCategoriesQueryValidator : AbstractValidator<SearchCategoriesQuery>
    {
        public SearchCategoriesQueryValidator()
        {
            RuleFor(a => a.Query)
                .NotNull();
        }
    }
}
