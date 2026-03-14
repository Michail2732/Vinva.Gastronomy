using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;

namespace Vinva.Gastronomy.Media.Persistence.Converters
{
    public class ImageInfoConverter: ValueConverter<ImageInfo, string>
    {
        public ImageInfoConverter() : base(to => to.ToString(), from => ImageInfo.Parce(from))
        {

        }
    }
}
