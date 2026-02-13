using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveSteps
{
    public class RemoveStepsCommandValidator : AbstractValidator<RemoveStepsCommand>
    {
        public RemoveStepsCommandValidator()
        {
            RuleFor(a => a.RecipeId).NotEmpty();
            RuleFor(a => a.SeqNumbers).NotEmpty();
        }
    }
}
