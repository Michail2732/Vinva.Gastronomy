using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.SearchByCharacteristics
{
    public sealed class SearchRecipesByCharacteristicsQueryValidator : AbstractValidator<SearchRecipesByCharacteristicsQuery>
    {
        public SearchRecipesByCharacteristicsQueryValidator()
        {
            RuleFor(query => query.Skip)
                .GreaterThanOrEqualTo(0);

            RuleFor(query => query.Take)
                .GreaterThanOrEqualTo(0);

            RuleForEach(query => query.Filters)
                .SetValidator(new CharacteristicFilterValidator());
        }
    }

    internal sealed class CharacteristicFilterValidator : AbstractValidator<CharacteristicFilterDto>
    {
        public CharacteristicFilterValidator()
        {
            RuleFor(filter => filter.Name)
                .NotEmpty();
        }
    }
}
