using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Media.Application.Common
{
    public class ImagesFileStorageConfig
    {
        public required string FilesDirectory { get; init; }
        /// <summary>
        /// Шаблон url для запроса изображения из CDN
        /// </summary>
        public required string ImagesUrlTemplate { get; set; }
    }
}
