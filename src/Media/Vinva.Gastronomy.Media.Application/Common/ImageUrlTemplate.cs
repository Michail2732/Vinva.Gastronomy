using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;

namespace Vinva.Gastronomy.Media.Application.Common
{
    public class ImageUrlTemplate
    {
        private readonly string _rawTemplate;


        public ImageUrlTemplate(string rawTemplate)
        {
            _rawTemplate = rawTemplate ?? throw new ArgumentNullException(nameof(rawTemplate));
        }


        public string CreateUrl(Guid imageId, ImageInfo info)
        {
            return string.Format(_rawTemplate, imageId, info.Width, info.Height, info.Quality, info.Format);
        }
    }
}
