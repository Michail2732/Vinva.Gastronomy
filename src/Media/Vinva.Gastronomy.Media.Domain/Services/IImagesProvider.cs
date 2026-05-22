using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;

namespace Vinva.Gastronomy.Media.Domain.Services
{
    public interface IImagesProvider
    {
        Task<Stream> DownloadAsync(Guid imageId, CancellationToken ct = default);
        Task<string> GetUrlAsync(Guid imageId, ImageInfo info, CancellationToken ct = default);
        Task<bool> IsExistsAsync(Guid imageId, CancellationToken ct = default);
    }
}
