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
    public interface IImagesStorage : IImagesProvider
    {                
        Task UploadAsync(Stream itemStream, Guid imageId, CancellationToken ct = default);        
        Task DeleteAsync(Guid imageId, CancellationToken ct = default);        
    }
}
