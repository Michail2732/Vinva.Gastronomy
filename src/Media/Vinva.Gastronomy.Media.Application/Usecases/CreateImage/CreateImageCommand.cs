using MediatR;

namespace Vinva.Gastronomy.Media.Application.Usecases.CreateImage
{   
    public record CreateImageCommand : IRequest<CreateImageCommandResponse>
    {
        public required string OwnerId { get; init; }
        public required Stream Content { get; init; }
        public required long Size { get; init; }
        public required string Name { get; init; }
        public string? Group { get; init; }        
        public required string Format { get; init; }        
    }
}