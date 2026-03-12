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

        public Task DeleteAsync(string path, CancellationToken ct = default)
        {
            File.Delete(path);
            return Task.CompletedTask;
        }

        public Task<Stream> DownloadAsync(string path, CancellationToken ct = default)
        {
            return Task.FromResult<Stream>(new FileStream(path, FileMode.Open));
        }

        public Task<string> GetUrlAsync(string path)
        {
            Path.GetRelativePath
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
