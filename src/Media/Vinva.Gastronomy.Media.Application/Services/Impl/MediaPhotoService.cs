using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;
using Vinva.Gastronomy.Media.Domain.Models;
using Vinva.Gastronomy.Media.Domain.Services;

namespace Vinva.Gastronomy.Media.Application.Services.Impl
{
    public class MediaPhotoService : IImagesService
    {
        private readonly IImagesStorage _mediaStorage;
        private readonly ImagesFormatConverter _imagesConverter;

        public MediaPhotoService(IImagesStorage storageService)
        {
            _mediaStorage = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _imagesConverter = new ImagesFormatConverter();
        }

        public async Task<string> GetProcessedUrlAsync(ImageMetadata item, ImageOptions imageOpts, CancellationToken ct = default)
        {            
            if (!item.Path.Format.Equals(imageOpts.TargetFormat, StringComparison.OrdinalIgnoreCase))
            {
                var itemNewPath = new ImagePath(item.Id, imageOpts.TargetFormat, imageOpts.Width, imageOpts.Height, imageOpts.Quality);                
                if (!(await _mediaStorage.IsExistsAsync(itemNewPath, ct)))
                {
                    var imageStream = await _mediaStorage.DownloadAsync(item.Path, ct);
                    var processedImageStream = await _imagesConverter.ConvertToFileAsync(imageStream, imageOpts);
                    await _mediaStorage.UploadAsync(processedImageStream, itemNewPath, ct);                    
                }
                string resultUrl = await _mediaStorage.GetUrlAsync(itemNewPath, ct);
                return resultUrl;
            }
            return await _mediaStorage.GetUrlAsync(item.Path, ct);
        }
    }
}
