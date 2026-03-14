using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Media.Domain.Models
{
    public class ImageOptions
    {
        public const int DefaultQuality = 75;
        public string TargetFormat { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Quality { get; private set; }

        public ImageOptions(string targetFormat, int width, int height, int? quality = null)
        {
            Width = width;
            Height = height;
            Quality = quality ?? DefaultQuality;
            TargetFormat = targetFormat ?? throw new ArgumentNullException(nameof(targetFormat));
        }
    }
}
