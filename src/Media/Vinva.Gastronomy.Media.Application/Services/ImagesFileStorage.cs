using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Application.Common;
using Vinva.Gastronomy.Media.Domain.Entities;
using Vinva.Gastronomy.Media.Domain.Exceptions;
using Vinva.Gastronomy.Media.Domain.Services;
using Vinva.Gastronomy.Media.Persistence;

namespace Vinva.Gastronomy.Media.Application.Services
{
    public class ImagesFileStorage : IImagesStorage
    {
        private readonly ImagesFileStorageConfig _configuraiton;

        public ImagesFileStorage(IOptions<ImagesFileStorageConfig> fileStorageConfiguration)
        {
            _configuraiton = fileStorageConfiguration?.Value ?? throw new ArgumentNullException(nameof(fileStorageConfiguration));
        }

        public Task CommitTempAsync(Guid imageId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid imageId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadAsync(Guid imageId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUrlAsync(Guid imageId, ImageInfo path, CancellationToken ct = default)
        {
            var urlTemplate = new ImageUrlTemplate(_configuraiton.ImageUrlTemplate);
            var url = urlTemplate.CreateUrl(imageId, path);
            return Task.FromResult(url);
        }

        public Task<bool> IsExistsAsync(Guid imageId, CancellationToken ct = default)
        {
            var filePath = Path.Combine(_configuraiton.ImagesDirectory, imageId.ToString());
            return Task.FromResult(File.Exists(filePath));
        }

        public async Task UploadTempAsync(Stream itemStream, Guid imageId, CancellationToken ct = default)
        {
            var newFilePath = Path.Combine(_configuraiton.ImagesTempDirectory, imageId.ToString());
            using var newFile = File.OpenWrite(newFilePath);
            await itemStream.CopyToAsync(newFile, ct);
        }
    }
}
