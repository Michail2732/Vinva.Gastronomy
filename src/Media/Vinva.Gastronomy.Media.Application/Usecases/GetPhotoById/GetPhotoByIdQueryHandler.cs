using MediatR;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetPhotoById
{
    internal sealed class GetPhotoByIdQueryHandler : IRequestHandler<GetPhotoByIdQuery, GetPhotoByIdQueryResponse>
    {
        public Task<GetPhotoByIdQueryResponse> Handle(GetPhotoByIdQuery request, CancellationToken cancellationToken)
        {
            // Implement your logic here
            throw new NotImplementedException();
        }
    }
}
