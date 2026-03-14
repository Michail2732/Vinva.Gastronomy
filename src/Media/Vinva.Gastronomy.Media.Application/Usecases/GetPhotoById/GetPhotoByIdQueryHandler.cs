using MediatR;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Media.Application.Errors;
using Vinva.Gastronomy.Media.Application.Services;
using Vinva.Gastronomy.Media.Application.Services.Impl;
using Vinva.Gastronomy.Media.Domain.Models;
using Vinva.Gastronomy.Media.Domain.Services;
using Vinva.Gastronomy.Media.Persistence;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetPhotoById
{
    public sealed class GetPhotoByIdQueryHandler : IRequestHandler<GetPhotoByIdQuery, GetPhotoByIdQueryResponse>
    {        
        private readonly IImagesService _imagesService;
        private readonly MediaDbContext _dbContext;


        public GetPhotoByIdQueryHandler(MediaDbContext dbContext, IImagesService imagesService)
        {            
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _imagesService = imagesService ?? throw new ArgumentNullException(nameof(imagesService));
        }


        public async Task<GetPhotoByIdQueryResponse> Handle(GetPhotoByIdQuery request, CancellationToken cancellationToken)
        {
            var formatConverter = new ImagesFormatConverter();

            var mediaItem = _dbContext.MediaItems.FirstOrDefault(a => a.Id == request.MediaId);

            if (mediaItem == null)
                throw new NotFoundException(MediaApplicationErrors.MediaNotFound);

            var imageOpts = new ImageOptions(request.Format, request.Width, request.Height, request.Quality);
            var imageUrl = await _imagesService.GetProcessedUrlAsync(mediaItem, imageOpts);
            return new GetPhotoByIdQueryResponse
            {
                Id = mediaItem.Id,
                Size = mediaItem.Size,
                Url = imageUrl,
                Format = request.Format
            };
        }
    }
}
