using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Media.Application.Errors
{
    public class MediaApplicationErrors
    {
        public static Error MediaNotFound => new("Media.MediaNotFound", "Не удалось найти медиа файл");

    }
}
