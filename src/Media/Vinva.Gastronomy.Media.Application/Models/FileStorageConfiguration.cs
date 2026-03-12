using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Media.Domain.Models
{
    public class FileStorageConfiguration
    {        
        public required string FilesDirectory { get; init; }
        public string? PublicPathPart { get; set; }
    }
}
