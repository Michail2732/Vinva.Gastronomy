using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;
using Vinva.Gastronomy.Media.Domain.Exceptions;
using Vinva.Gastronomy.Media.Domain.Models;
using Vinva.Gastronomy.Media.Domain.Services;
using Vinva.Gastronomy.Media.Persistence;

namespace Vinva.Gastronomy.Media.Application.Services.Impl
{
    public class ImagesFileStorage : IImagesStorage
    {        
        private readonly ImagesFileStorageConfig _configuraiton;


        public ImagesFileStorage(IOptions<ImagesFileStorageConfig> fileStorageConfiguration)
        {     
            _configuraiton = fileStorageConfiguration?.Value ?? throw new ArgumentNullException(nameof(fileStorageConfiguration));
        }        

        public Task DeleteAsync(ImagePath path, CancellationToken ct = default)
        {
            var filePath = Path.Combine(_configuraiton.FilesDirectory, path.ToString());
            File.Delete(filePath);
            return Task.CompletedTask;
        }

        public Task<Stream> DownloadAsync(ImagePath path, CancellationToken ct = default)
        {
            var filePath = Path.Combine(_configuraiton.FilesDirectory, path.ToString());
            return Task.FromResult<Stream>(new FileStream(filePath, FileMode.Open));
        }

        public Task<string> GetUrlAsync(ImagePath path, CancellationToken ct = default)
        {                        
            var resultPath = Path.Combine(_configuraiton.MapedPath, path.ToString());
            return Task.FromResult(resultPath);
        }

        public Task<bool> IsExistsAsync(ImagePath path, CancellationToken ct = default)
        {
            var filePath = Path.Combine(_configuraiton.FilesDirectory, path.ToString());
            return Task.FromResult(File.Exists(filePath));
        }

        public async Task UploadAsync(Stream itemStream, ImagePath path, CancellationToken ct = default)
        {
            var newFilePath = Path.Combine(_configuraiton.FilesDirectory, path.ToString());
            using var newFile = File.OpenWrite(newFilePath);
            await itemStream.CopyToAsync(newFile, ct);            
        }
    }
}
