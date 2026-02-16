using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.IngredientUsecases.RemoveIngredients
{
    public class RemoveIngredientsCommandValidator : AbstractValidator<RemoveIngredientsCommand>
    {
        public RemoveIngredientsCommandValidator()
        {
            RuleFor(a => a.IngredientIds).NotEmpty();
        }
    }
}
