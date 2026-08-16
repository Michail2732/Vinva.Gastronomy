using MediatR;
using Vinva.Gastronomy.Media.Domain.Entities;
using Vinva.Gastronomy.Media.Domain.Services;
using Vinva.Gastronomy.Media.Persistence;
using Vinva.Gastronomy.Media.WebApi.Services;

namespace Vinva.Gastronomy.Media.Application.Usecases.CreateImage
{
    public sealed class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, CreateImageCommandResponse>
    {
        private readonly IImagesStorage _imagesStorage;
        private readonly IImageFormatService _formatService;
        private readonly MediaDbContext _dbContext;

        public CreateImageCommandHandler(IImagesStorage imagesStorage, 
            MediaDbContext dbContext, IImageFormatService formatService)
        {
            _imagesStorage = imagesStorage ?? throw new ArgumentNullException(nameof(imagesStorage));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _formatService = formatService ?? throw new ArgumentNullException(nameof(formatService));
        }

        //todo: Добавить валидацию содержимого файлов
        public async Task<CreateImageCommandResponse> Handle(CreateImageCommand request, CancellationToken ct)
        {
            var imageFormat = await _formatService.GetFormatAsync(request.Content, request.ContentType, ct);
            var imageMeta = new ImageMetadata(request.FileName)
            {
                Format = imageFormat,
                ContentType = request.ContentType,
                OwnerId = request.OwnerId,
                Size = request.Size
            };

            await _imagesStorage.UploadTempAsync(request.Content, imageMeta.Id, ct);
            await _dbContext.Images.AddAsync(imageMeta, ct);

            return new CreateImageCommandResponse
            {
                ImageId = imageMeta.Id
            };
        }
    }
}