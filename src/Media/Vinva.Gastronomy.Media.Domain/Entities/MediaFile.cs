using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;

namespace Vinva.Gastronomy.Media.Domain.Entities
{
    public class MediaFile : EntityOfT<Guid>
    {
        public string Name { get; private set; }
        public string ContentType { get; private set; } 
        public long Size { get; private set; }
        public string StoragePath { get; private set; } 
        public DateTime UploadedAt { get; private set; }
        public MediaFileState State { get; private set; }

#pragma warning disable CS8618
        private MediaFile()
        {

        }
#pragma warning restore CS8618
        public MediaFile(Guid id, string name, string contentType, long size, string storagePath, DateTime uploadedAt, MediaFileState state = MediaFileState.None)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
            Size = size;
            StoragePath = storagePath ?? throw new ArgumentNullException(nameof(storagePath));
            UploadedAt = uploadedAt;
            State = state;
        }        
    }
}
