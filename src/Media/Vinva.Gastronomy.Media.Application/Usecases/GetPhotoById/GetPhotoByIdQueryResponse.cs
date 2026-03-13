namespace Vinva.Gastronomy.Media.Application.Usecases.GetPhotoById
{
    public record GetPhotoByIdQueryResponse 
    {
        public Guid Id { get; set; }        
        public required string Url { get; init; }
        public required string Format { get; init; }
        public long Size { get; set; }        
    }
}
