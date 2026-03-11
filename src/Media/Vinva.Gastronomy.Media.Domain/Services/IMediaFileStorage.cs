using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;

namespace Vinva.Gastronomy.Media.Domain.Services
{
    public interface IMediaFileStorage
    {
        Task<Guid> UploadAsync(Stream fs, string name, string contentType, CancellationToken ct = default);
        Task<Stream> DownloadAsync(Guid mediaId, CancellationToken ct = default);
        Task<MediaFile> GetByIdAsync(Guid mediaId, CancellationToken ct = default);
        Task DeleteAsync(Guid mediaId, CancellationToken ct = default);
    }
}
