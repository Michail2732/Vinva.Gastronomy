using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveIngredients
{
    public class RemoveIngredientsCommandValidator : AbstractValidator<RemoveIngredientsCommand>
    {
        public RemoveIngredientsCommandValidator()
        {
            RuleFor(a => a.RecipeId).NotEmpty();
            RuleFor(a => a.IngredientIds).NotEmpty();
        }
    }
}
