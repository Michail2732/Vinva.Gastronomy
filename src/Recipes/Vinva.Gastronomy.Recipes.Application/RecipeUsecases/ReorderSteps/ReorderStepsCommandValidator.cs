using FluentValidation;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.ReorderSteps
{
    public class ReorderStepsCommandValidator : AbstractValidator<ReorderStepsCommand>
    {
        public ReorderStepsCommandValidator()
        {
            RuleFor(a => a.RecipeId)
                .NotEmpty();

            RuleFor(a => a.Items)
                .NotEmpty();

            RuleForEach(a => a.Items)
                .ChildRules(a =>
                {
                    a.RuleFor(b => b.SeqNumber1)
                     .Must(b => b > 0);

                    a.RuleFor(b => b.SeqNumber2)
                     .Must(b => b > 0);

                    a.RuleFor(b => new { b.SeqNumber1, b.SeqNumber2 })
                     .Must(b => b.SeqNumber1 != b.SeqNumber2);
                });
        }
    }
}
