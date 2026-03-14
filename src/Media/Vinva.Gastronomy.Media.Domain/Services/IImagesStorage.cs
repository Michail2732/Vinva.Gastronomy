using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;

namespace Vinva.Gastronomy.Media.Domain.Services
{
    /// <summary>
    /// Хранилище файлов
    /// </summary>
    public interface IImagesStorage
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemStream"></param>
        /// <param name="path"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task UploadAsync(Stream itemStream, ImagePath path, CancellationToken ct = default);
        Task<Stream> DownloadAsync(ImagePath path, CancellationToken ct = default);
        Task DeleteAsync(ImagePath path, CancellationToken ct = default);
        Task<string> GetUrlAsync(ImagePath path, CancellationToken ct = default);
        Task<bool> IsExistsAsync(ImagePath path, CancellationToken ct = default);
    }
}
