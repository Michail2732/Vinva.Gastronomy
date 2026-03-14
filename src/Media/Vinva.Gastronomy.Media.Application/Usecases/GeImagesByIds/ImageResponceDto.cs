namespace Vinva.Gastronomy.Media.Application.Usecases.GetImagesByIds
{
    public class ImageResponceDto
    {
        public Guid ImageId { get; init; }
        public required string Url { get; init; }        
    }
}
