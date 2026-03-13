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
    public class MediaItemLocalFileStorage : IMediaItemStorage
    {        
        private readonly FileStorageConfiguration _configuraiton;


        public MediaItemLocalFileStorage(IOptions<FileStorageConfiguration> fileStorageConfiguration)
        {     
            _configuraiton = fileStorageConfiguration?.Value ?? throw new ArgumentNullException(nameof(fileStorageConfiguration));
        }


        public Task<string> CreatePathAsync(Guid id, string name, string format, CancellationToken ct = default)
        {            
            var invalidPathChars = Path.GetInvalidPathChars()
                                       .Union(Path.GetInvalidFileNameChars())
                                       .Distinct()
                                       .ToArray();            
            
            name = string.Concat(name.Split(invalidPathChars));
            format = string.Concat(format.Split(invalidPathChars));

            string newItemPath = $"{id}_{name}_{format}";            
            return Task.FromResult(newItemPath);
        }

        public Task DeleteAsync(string path, CancellationToken ct = default)
        {
            File.Delete(path);
            return Task.CompletedTask;
        }

        public Task<Stream> DownloadAsync(string path, CancellationToken ct = default)
        {
            return Task.FromResult<Stream>(new FileStream(path, FileMode.Open));
        }

        public Task<string> GetUrlAsync(string path, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(_configuraiton.PublicPathPart))
                throw new MediaDomainException($"Url for file storage doesn't configured: {nameof(FileStorageConfiguration.PublicPathPart)}");
            var fileName = Path.GetFileName(path);            
            var relativePath = Path.GetRelativePath(_configuraiton.FilesDirectory, path);
            var resultPath = Path.Combine(_configuraiton.PublicPathPart, path);
            return Task.FromResult(resultPath);
        }

        public Task<bool> IsExistsAsync(string path, CancellationToken ct = default)
        {
            var filePath = Path.Combine(_configuraiton.FilesDirectory, path);
            return Task.FromResult(File.Exists(filePath));
        }

        public async Task<string> UploadAsync(Stream itemStream, string name, CancellationToken ct = default)
        {
            var newFilePath = Path.Combine(_configuraiton.FilesDirectory, name);
            using var newFile = File.OpenWrite(newFilePath);
            await itemStream.CopyToAsync(newFile, ct);
            return newFilePath;
        }
    }
}
