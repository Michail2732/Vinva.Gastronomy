using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Media.Domain.Models
{
    public class ImagesFileStorageConfig
    {        
        public required string FilesDirectory { get; init; }
        public required string MapedPath { get; set; }
    }
}
