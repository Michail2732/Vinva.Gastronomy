using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Remove
{
    public sealed class RemoveCommandValidator : AbstractValidator<RemoveCommand>
    {
        public RemoveCommandValidator()
        {
            RuleFor(command => command.RecipeId).NotEmpty();
        }
    }
}
