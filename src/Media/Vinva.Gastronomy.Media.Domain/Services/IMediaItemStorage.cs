using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;

namespace Vinva.Gastronomy.Media.Domain.Services
{
    public interface IMediaItemStorage
    {        
        Task<string> UploadAsync(Stream itemStream, string name, CancellationToken ct = default);        
        Task<Stream> DownloadAsync(string path, CancellationToken ct = default);        
        Task DeleteAsync(string path, CancellationToken ct = default);
        Task<string> GetUrlAsync(string path);        
    }
}
