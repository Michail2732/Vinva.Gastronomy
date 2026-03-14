using FluentValidation;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Common.Validations;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Create
{
    public class CreateRecipeCommandValidator : AbstractValidator<CreateRecipeCommand>
    {
        public CreateRecipeCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .Must(DescriptiveEntityValidator.ValidateName)
                .WithMessage(CommonErrorMessages.IncorrectName);

            RuleFor(a => a.Description)
                .NotEmpty()                                
                .Must(DescriptiveEntityValidator.ValidateDescription)
                .WithMessage(CommonErrorMessages.IncorrectDescription);

            RuleFor(a => a.CookingTime)
                .NotEmpty()
                .Must(RecipeDomainValidator.ValidateCookingTime)
                .WithMessage(RecipeDomainErrors.IncorrectCookingTime);

            RuleFor(a => a.Comment)                
                .Must(a => a == null || DescriptiveEntityValidator.ValidateComment(a))
                .WithMessage(CommonErrorMessages.IncorrectComment);

            RuleFor(a => a.StorageComment)
                .Must(a => a == null || DescriptiveEntityValidator.ValidateComment(a))
                .WithMessage(CommonErrorMessages.IncorrectComment);

            RuleFor(a => a.UsageComment)
                .Must(a => a == null || DescriptiveEntityValidator.ValidateComment(a))
                .WithMessage(CommonErrorMessages.IncorrectComment);            
        }
    }
}
