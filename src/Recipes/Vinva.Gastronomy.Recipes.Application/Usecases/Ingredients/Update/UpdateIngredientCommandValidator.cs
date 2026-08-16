using FluentValidation;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Update
{
    public class UpdateIngredientCommandValidator : AbstractValidator<UpdateIngredientCommand>
    {
        public UpdateIngredientCommandValidator()
        {
            When(a => !string.IsNullOrEmpty(a.Name), () =>
            {
                RuleFor(a => a.Name)
                .Must(RecipeDomainValidator.ValidateName!)
                .WithMessage(RecipeDomainErrors.IncorrectName);
            });

            When(a => !string.IsNullOrEmpty(a.Description), () =>
            {
                RuleFor(a => a.Description)
                .Must(RecipeDomainValidator.ValidateDescription!)
                .WithMessage(RecipeDomainErrors.IncorrectDescription);
            });                        
        }
    }
}
