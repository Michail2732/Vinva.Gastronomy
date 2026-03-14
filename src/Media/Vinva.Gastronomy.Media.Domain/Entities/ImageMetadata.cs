using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common;

namespace Vinva.Gastronomy.Media.Domain.Entities
{
    public class ImageMetadata : EntityOfT<Guid>
    {
        public string Name { get; private set; }        
        
        public required string Format { get; init; }

        public required string ContentType { get; init; }        

        public required string OwnerId { get; init; } 

        public required long Size { get; init; }        

        public DateTime UploadedAt { get; init; }        

        public string? Group { get; set; }


#pragma warning disable CS8618
        private ImageMetadata()
        {

        }
#pragma warning restore CS8618
        public ImageMetadata(Guid id, string name) 
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));            
        }

        public ImageMetadata(string name) : base()
        {            
            Name = name ?? throw new ArgumentNullException(nameof(name));            
        }

    }
}
