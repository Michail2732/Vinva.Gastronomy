using MediatR;
using Microsoft.EntityFrameworkCore;
using Vinva.Gastronomy.Common.Infrastructure.Exceptions;
using Vinva.Gastronomy.Media.Application.Common.Errors;
using Vinva.Gastronomy.Media.Domain.Entities;
using Vinva.Gastronomy.Media.Domain.Services;
using Vinva.Gastronomy.Media.Persistence;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetImagesByIds
{
    internal sealed class GetImagesByIdsQueryHandler : IRequestHandler<GetImagesByIdsQuery, GetImagesByIdsQueryResponse>
    {
        private readonly IImagesStorage _imagesStorage;
        private readonly MediaDbContext _dbContext;


        public GetImagesByIdsQueryHandler(MediaDbContext dbContext, IImagesStorage imagesStorage)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _imagesStorage = imagesStorage ?? throw new ArgumentNullException(nameof(imagesStorage));
        }


        public async Task<GetImagesByIdsQueryResponse> Handle(GetImagesByIdsQuery request, CancellationToken ct)
        {

            var imageIds = request.ImageDtos.Select(a => a.ImageId).Distinct().ToList();
            var images = await _dbContext.MediaItems.Where(a => imageIds.Contains(a.Id))
                                .ToListAsync(ct);

            var imageResponceDtos = new List<ImageResponceDto>(imageIds.Count);
            foreach (var imageId in imageIds)
            {
                var image = images.FirstOrDefault(a => a.Id == imageId);
                var imageRequestDto = request.ImageDtos.First(a => a.ImageId == imageId);
                if (image == null)
                    throw new NotFoundException(MediaApplicationErrors.ImageNotFound(imageId));

                var imageInfo = imageRequestDto.MapToInfo();
                var url = await _imagesStorage.GetUrlAsync(imageRequestDto.ImageId, imageInfo, ct);

                imageResponceDtos.Add(new ImageResponceDto
                {
                    ImageId = image.Id,
                    Url = url
                });
            }

            return new GetImagesByIdsQueryResponse
            {
                Items = imageResponceDtos.ToArray()
            };
        }
    }
}
