using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Remove
{
    public class RemoveIngredientsCommandValidator : AbstractValidator<RemoveIngredientsCommand>
    {
        public RemoveIngredientsCommandValidator()
        {
            RuleFor(a => a.IngredientIds).NotEmpty();
        }
    }
}
