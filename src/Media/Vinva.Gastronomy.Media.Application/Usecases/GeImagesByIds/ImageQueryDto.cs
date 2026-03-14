using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;

namespace Vinva.Gastronomy.Media.Application.Usecases.GetImagesByIds
{
    public class ImageQueryDto
    {
        public required Guid ImageId { get; init; }
        public required int Width { get; init; }
        public required int Height { get; init; }
        public required int Quality { get; init; } = 75;
        public required string Format { get; init; }

        public ImageInfo MapToInfo()
        {
            return new ImageInfo(Format, Width, Height, Quality);
        }
    }
}
