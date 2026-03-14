using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Media.Domain.Errors
{
    public class MediaErrorMessages
    {
        public static string IncorrectImageFormat(string format) => $"Некорректный формат изображения '{format}'";

        public static string IncorrectRawImagePath(string str) => $"Некорректный строка пути изображения '{str}'";
    }
}
