using MediatR;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetVideosByIds
{
    internal sealed class GetVideosByIdsCommandHandler : IRequestHandler<GetVideosByIdsCommand, GetVideosByIdsCommandResponse>
    {
        public Task<GetVideosByIdsCommandResponse> Handle(GetVideosByIdsCommand request, CancellationToken cancellationToken)
        {
            // Implement your logic here
            throw new NotImplementedException();
        }
    }
}