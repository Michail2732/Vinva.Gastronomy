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
        /// <summary>
        /// Загружает файл изображения во временное хранилище
        /// </summary>
        /// <param name="itemStream"></param>
        /// <param name="imageId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task UploadTempAsync(Stream itemStream, Guid imageId, CancellationToken ct = default);
        /// <summary>
        /// Отправляет запрос на перенос файла из временного хранилища в постоянное
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task CommitTempAsync(Guid imageId, CancellationToken ct = default);
        /// <summary>
        /// Удаляет файл изображения из постоянного хранилища
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid imageId, CancellationToken ct = default);        
    }
}
