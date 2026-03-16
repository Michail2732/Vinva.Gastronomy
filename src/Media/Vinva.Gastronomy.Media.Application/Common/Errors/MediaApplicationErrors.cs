using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Media.Application.Common.Errors
{
    public class MediaApplicationErrors
    {
        public static Error ImageNotFound(Guid imageId) => new("Media.ImageNotFound", $"Не удалось найти изображение '{imageId}'");

        public const string IncorrectImageFormat = "Некорректный формат изображения";
        public const string ImageWidthMustBeLargeThen0 = "Ширина изображения должна быть больше нуля";
        public const string ImageHeightMustBeLargeThen0 = "Высота изображения должна быть больше нуля";
        public const string IncorrectImageQuality = "Качество изображения должно быть в диапозоне от 0 до 100";

    }
}
