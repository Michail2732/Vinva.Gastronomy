using MediatR;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetImagesByIds
{
    public record GetImagesByIdsQuery : IRequest<GetImagesByIdsQueryResponse>
    {
        public required ImageQueryDto[] ImageDtos { get; init; }
    }
}
