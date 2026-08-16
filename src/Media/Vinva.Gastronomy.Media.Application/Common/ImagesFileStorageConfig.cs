using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Media.Application.Common
{
    public class ImagesFileStorageConfig
    {
        public required string ImagesTempDirectory { get; init; }
        public required string ImagesDirectory { get; init; }
        public required string ImageUrlTemplate { get; init; }
    }
}
