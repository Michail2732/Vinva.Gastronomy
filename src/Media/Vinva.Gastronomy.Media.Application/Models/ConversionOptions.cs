using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Media.Domain.Models
{
    public class ConversionOptions
    {
        public string TargetFormat { get; private set; }
        public int? Width { get; private set; }
        public int? Height { get; private set; }
        public int Quality { get; private set; }

        public ConversionOptions(string targetFormat, int ? width, int? height, int quality = 75)
        {
            Width = width;
            Height = height;
            Quality = quality;
            TargetFormat = targetFormat ?? throw new ArgumentNullException(nameof(targetFormat));
        }
    }
}
