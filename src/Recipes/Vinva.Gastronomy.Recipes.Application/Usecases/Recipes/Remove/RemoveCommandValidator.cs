using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Remove
{
    public class RemoveCommandValidator : AbstractValidator<RemoveCommand>
    {
        public RemoveCommandValidator()
        {
            // Add validation rules here
        }
    }
}
