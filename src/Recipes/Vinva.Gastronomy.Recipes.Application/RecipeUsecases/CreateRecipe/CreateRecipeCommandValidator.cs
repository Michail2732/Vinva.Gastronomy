using FluentValidation;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Application.RecipeUsecases.CreateRecipe
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .Must(RecipeDomainValidator.ValidateName)
                .WithMessage(RecipeDomainErrors.IncorrectName);

            RuleFor(a => a.Description)
                .NotEmpty()                                
                .Must(RecipeDomainValidator.ValidateDescription)
                .WithMessage(RecipeDomainErrors.IncorrectDescription);

            RuleFor(a => a.CookingTime)
                .NotEmpty()
                .Must(RecipeDomainValidator.ValidateCookingTime)
                .WithMessage(RecipeDomainErrors.IncorrectCookingTime);

            RuleFor(a => a.Comment)                
                .Must(a => a == null || RecipeDomainValidator.ValidateComment(a))
                .WithMessage(RecipeDomainErrors.IncorrectComment);

            RuleFor(a => a.StorageComment)
                .Must(a => a == null || RecipeDomainValidator.ValidateComment(a))
                .WithMessage(RecipeDomainErrors.IncorrectComment);

            RuleFor(a => a.UsageComment)
                .Must(a => a == null || RecipeDomainValidator.ValidateComment(a))
                .WithMessage(RecipeDomainErrors.IncorrectComment);            
        }
    }
}
