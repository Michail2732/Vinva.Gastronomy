using FluentValidation;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Create
{
    public class CreateIngredientCommandValidator : AbstractValidator<CreateIngredientCommand>
    {
        public CreateIngredientCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .Must(RecipeDomainValidator.ValidateName)
                .WithMessage(RecipeDomainErrors.IncorrectName);

            RuleFor(a => a.Description)
                .NotEmpty()
                .Must(RecipeDomainValidator.ValidateDescription)
                .WithMessage(RecipeDomainErrors.IncorrectDescription);            

            RuleFor(a => a.Comment)
                .Must(a => a == null || RecipeDomainValidator.ValidateComment(a))
                .WithMessage(RecipeDomainErrors.IncorrectComment);
        }
    }
}
