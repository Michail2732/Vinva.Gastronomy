using FluentValidation;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Create
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .Must(ValidationService.ValidateName)
                .WithMessage(CommonErrorMessages.IncorrectName);

            RuleFor(a => a.Description)
                .NotEmpty()                                
                .Must(ValidationService.ValidateDescription)
                .WithMessage(CommonErrorMessages.IncorrectDescription);

            RuleFor(a => a.CookingTime)
                .NotEmpty()
                .Must(RecipeDomainValidator.ValidateCookingTime)
                .WithMessage(RecipeDomainErrors.IncorrectCookingTime);

            RuleFor(a => a.Comment)                
                .Must(a => a == null || ValidationService.ValidateComment(a))
                .WithMessage(CommonErrorMessages.IncorrectComment);

            RuleFor(a => a.StorageComment)
                .Must(a => a == null || ValidationService.ValidateComment(a))
                .WithMessage(CommonErrorMessages.IncorrectComment);

            RuleFor(a => a.UsageComment)
                .Must(a => a == null || ValidationService.ValidateComment(a))
                .WithMessage(CommonErrorMessages.IncorrectComment);            
        }
    }
}
