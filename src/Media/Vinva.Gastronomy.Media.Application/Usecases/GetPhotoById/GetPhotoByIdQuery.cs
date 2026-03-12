using MediatR;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetPhotoById
{
    public record GetPhotoByIdQuery : IRequest<GetPhotoByIdQueryResponse>
    {
        public Guid MediaId { get; init; }
        public int? Width { get; init; }
        public int? Height { get; init; }
        public int? Quality { get; init; }          
    }
}
