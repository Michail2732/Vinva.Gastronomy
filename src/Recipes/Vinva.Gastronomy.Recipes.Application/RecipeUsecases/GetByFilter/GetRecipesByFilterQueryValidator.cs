using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetByFilter
{
    public class GetRecipesByFilterQueryValidator : AbstractValidator<GetRecipesByFilterQuery>
    {
        public GetRecipesByFilterQueryValidator()
        {
            RuleFor(a => a.Query)
                .NotNull();            
        }
    }
}
