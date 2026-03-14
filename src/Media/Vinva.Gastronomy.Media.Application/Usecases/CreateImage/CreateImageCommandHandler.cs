using MediatR;
using Vinva.Gastronomy.Media.Domain.Entities;
using Vinva.Gastronomy.Media.Domain.Services;
using Vinva.Gastronomy.Media.Persistence;

namespace Vinva.Gastronomy.Media.Application.Usecases.CreateImage
{
    public sealed class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, CreateImageCommandResponse>
    {
        private readonly IImagesStorage _imagesStorage;
        private readonly MediaDbContext _dbContext;

        public CreateImageCommandHandler(IImagesStorage imagesStorage, MediaDbContext dbContext)
        {
            _imagesStorage = imagesStorage ?? throw new ArgumentNullException(nameof(imagesStorage));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<CreateImageCommandResponse> Handle(CreateImageCommand request, CancellationToken ct)
        {
            var imageMeta = new ImageMetadata(request.Name)
            {
                Format = request.Format,
                ContentType = request.Format,
                OwnerId = request.OwnerId,
                Size = request.Size
            };

            await _imagesStorage.UploadAsync(request.Content, imageMeta.Id, ct);
            await _dbContext.MediaItems.AddAsync(imageMeta, ct);

            return new CreateImageCommandResponse
            {
                ImageId = imageMeta.Id
            };
        }
    }
}