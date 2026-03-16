using FluentValidation;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Common.Validations;
using Vinva.Gastronomy.Media.Application.Common.Errors;

namespace Vinva.Gastronomy.Media.Application.Usecases.CreateImage
{
    public class CreateImageCommandValidator : AbstractValidator<CreateImageCommand>
    {
        public CreateImageCommandValidator()
        {
            RuleFor(a => a.Content)
                .NotEmpty()
                .Must(a => a.CanRead);

            RuleFor(a => a.ContentType)
                .NotEmpty();

            RuleFor(a => a.FileName)
                .NotEmpty()
                .Must(DescriptiveEntityValidator.ValidateName)
                .WithMessage(CommonErrorMessages.IncorrectName);            
        }
    }
}
