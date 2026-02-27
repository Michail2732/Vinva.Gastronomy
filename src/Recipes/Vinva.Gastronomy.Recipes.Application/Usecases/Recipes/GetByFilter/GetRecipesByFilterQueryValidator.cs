using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByFilter
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
