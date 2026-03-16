using FluentValidation;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetVideosByIds
{
    public class GetVideosByIdsCommandValidator : AbstractValidator<GetVideosByIdsCommand>
    {
        public GetVideosByIdsCommandValidator()
        {
            // Add validation rules here
        }
    }
}
