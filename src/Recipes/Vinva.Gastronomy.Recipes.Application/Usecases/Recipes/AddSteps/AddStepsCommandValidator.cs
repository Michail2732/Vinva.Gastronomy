using FluentValidation;
using Vinva.Gastronomy.Recipes.Application.Common;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddSteps
{
    public class AddStepsCommandValidator : AbstractValidator<AddStepsCommand>
    {
        public AddStepsCommandValidator()
        {
            RuleFor(a => a.RecipeId)
                .NotEmpty();

            RuleFor(a => a.Steps)
                .NotEmpty();

            RuleForEach(a => a.Steps)
                .ChildRules(a =>
                {
                    a.RuleFor(b => b.Description)
                        .NotEmpty()
                        .Must(RecipeDomainValidator.ValidateDescription)
                        .WithMessage(RecipeDomainErrors.IncorrectDescription);
                });
        }
    }    
}
