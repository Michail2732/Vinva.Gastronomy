using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Media.Application.Errors;
using Vinva.Gastronomy.Media.Application.Services.Impl;
using Vinva.Gastronomy.Media.Domain.Services;
using Vinva.Gastronomy.Media.Persistence;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetPhotoById
{
    public sealed class GetPhotoByIdQueryHandler : IRequestHandler<GetPhotoByIdQuery, GetPhotoByIdQueryResponse>
    {
        private readonly IMediaItemStorage _mediaStorage;
        private readonly MediaDbContext _dbContext;


        public GetPhotoByIdQueryHandler(IMediaItemStorage mediaStorage, MediaDbContext dbContext)
        {
            _mediaStorage = mediaStorage ?? throw new ArgumentNullException(nameof(mediaStorage));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<GetPhotoByIdQueryResponse> Handle(GetPhotoByIdQuery request, CancellationToken cancellationToken)
        {
            var formatConverter = new ImagesFormatConverter();

            var mediaItem = _dbContext.MediaItems.FirstOrDefault(a => a.Id == request.MediaId);

            if (mediaItem == null)
                throw new NotFoundException(MediaApplicationErrors.MediaNotFound);
            
            if (!mediaItem.Format.Equals(request.Format, StringComparison.OrdinalIgnoreCase))
            {
                var mediaItemPath = await _mediaStorage.CreatePathAsync(mediaItem.Id, mediaItem.Name, request.Format);
                if (!(await _mediaStorage.IsExistsAsync(mediaItemPath, cancellationToken)))
                {
                    formatConverter.ConvertToFileAsync()
                }
            }

            _mediaStorage.IsExistsAsync()
        }
    }
}
