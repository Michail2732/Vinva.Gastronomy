using MediatR;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetVideosByIds
{
    // Include properties to be used as input for the command
    public record GetVideosByIdsCommand() : IRequest<GetVideosByIdsCommandResponse>;
}