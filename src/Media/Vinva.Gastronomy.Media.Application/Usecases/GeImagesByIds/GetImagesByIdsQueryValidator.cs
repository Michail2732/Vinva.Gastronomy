using FluentValidation;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Media.Application.Common.Errors;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetImagesByIds
{
    public class GetImagesByIdsQueryValidator : AbstractValidator<GetImagesByIdsQuery>
    {
        public GetImagesByIdsQueryValidator()
        {
            RuleFor(a => a.ImageDtos)
                .NotEmpty();

            RuleForEach(a => a.ImageDtos)                
                .ChildRules(a => {
                    a.RuleFor(b => b.Width)
                     .Must(b => b > 0)
                     .WithMessage(MediaApplicationErrors.ImageWidthMustBeLargeThen0);

                    a.RuleFor(b => b.Height)
                     .Must(b => b > 0)
                     .WithMessage(MediaApplicationErrors.ImageHeightMustBeLargeThen0);

                    a.RuleFor(b => b.Quality)
                     .Must(b => b > 0 && b <= 100)
                     .WithMessage(MediaApplicationErrors.IncorrectImageQuality);

                    a.RuleFor(b => b.Format)
                     .NotEmpty()
                     .Must(ValidationService.ValidateName)
                     .WithMessage(MediaApplicationErrors.IncorrectImageFormat);
                });            
        }
    }
}
