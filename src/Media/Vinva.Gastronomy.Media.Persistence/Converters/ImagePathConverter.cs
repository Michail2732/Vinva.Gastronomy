using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;

namespace Vinva.Gastronomy.Media.Persistence.Converters
{
    public class ImagePathConverter: ValueConverter<ImagePath, string>
    {
        public ImagePathConverter() : base(to => to.ToString(), from => ImagePath.Parce(from))
        {

        }
    }
}
