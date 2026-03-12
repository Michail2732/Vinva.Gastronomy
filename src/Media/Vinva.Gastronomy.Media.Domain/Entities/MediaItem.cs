using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;

namespace Vinva.Gastronomy.Media.Domain.Entities
{
    public class MediaItem : EntityOfT<Guid>
    {
        public string Name { get; private set; }        

        public string Path { get; private set; }        

        public required string ContentType { get; init; }

        public required string OwnerId { get; init; } 

        public required long Size { get; init; }

        public required MediaItemState State { get; set; }

        public DateTime UploadedAt { get; init; }        

        public string? Group { get; set; }


#pragma warning disable CS8618
        private MediaItem()
        {

        }
#pragma warning restore CS8618
        public MediaItem(Guid id, MediaStorage storage, string name, string path)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));                        
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Storage = storage;
        }        
    }
}
