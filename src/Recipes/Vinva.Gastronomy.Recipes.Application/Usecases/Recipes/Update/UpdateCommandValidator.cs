using FluentValidation;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Common.Models;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Recipes.Application.Common.Validators;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Update
{
    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(a => a.Name)              
              .Must(ValidationService.ValidateName!)
              .WithMessage(CommonErrorMessages.IncorrectName)
              .When(a => !string.IsNullOrEmpty(a.Name));

            RuleFor(a => a.Description)
              .Must(ValidationService.ValidateDescription!)
              .WithMessage(CommonErrorMessages.IncorrectDescription)
              .When(a => !string.IsNullOrEmpty(a.Description)); 

            RuleFor(a => a.CookingTime)
               .Must(a => RecipeDomainValidator.ValidateCookingTime(a!.Value))
               .WithMessage(RecipeDomainErrors.IncorrectCookingTime)
               .When(a => a.CookingTime.HasValue);

            RuleFor(a => a.Comment)
                .Must(a => ValidationService.ValidateComment(a!))
                .WithMessage(CommonErrorMessages.IncorrectComment)
                .When(a => !string.IsNullOrEmpty(a.Comment));

            RuleFor(a => a.StorageComment)
                .Must(a => ValidationService.ValidateComment(a!))
                .WithMessage(CommonErrorMessages.IncorrectComment)
                .When(a => !string.IsNullOrEmpty(a.StorageComment));

            RuleFor(a => a.UsageComment)
                .Must(a => ValidationService.ValidateComment(a!))
                .WithMessage(CommonErrorMessages.IncorrectComment)
                .When(a => !string.IsNullOrEmpty(a.UsageComment));


            RuleFor(a => a.CookingComment)
                .Must(a => ValidationService.ValidateComment(a!))
                .WithMessage(CommonErrorMessages.IncorrectComment)
                .When(a => !string.IsNullOrEmpty(a.CookingComment));

            RuleForEach(a => a.Ingredients)
                .SetValidator(new RecipeIngredientDtoValidator());

            RuleForEach(a => a.Steps)
                .SetValidator(new RecipeStepDtoValidator());

            RuleFor(a => a.Properties)
                .Must(a => a.Select(b => b.Name).Distinct().Count() == a.Length && 
                           a.All(b => b.Values.Count > 0))
                .When(a => a.Properties != null);

        }
    }
}
