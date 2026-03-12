using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Models;

namespace Vinva.Gastronomy.Media.Application.Services.Impl
{
    public class ImagesFormatConverter
    {                
        public async Task ConvertToFileAsync(string inputFilePath, string outputFilePath, 
            ConversionOptions options, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException(null, inputFilePath);

            using var image = await Image.LoadAsync(inputFilePath, ct);
            if (options.Width.HasValue || options.Height.HasValue )
            {
                image.Mutate(op =>
                {
                    op.Resize(new ResizeOptions
                    {
                        Size = new Size
                        {
                            Width = options.Width.HasValue ? options.Width.Value : image.Width,
                            Height = options.Height.HasValue ? options.Height.Value : image.Height
                        },
                        Mode = ResizeMode.Max,
                        Sampler = KnownResamplers.Lanczos3
                    });
                });
            }

            var prepareTargetFormat = options.TargetFormat.Trim().ToLower();
            var targetEncoder = CreateCoder(prepareTargetFormat, options.Quality);            
            
            await image.SaveAsync(outputFilePath, targetEncoder, ct);
        }

        private IImageEncoder CreateCoder(string preparedFormat, int quality)
        {
            return preparedFormat switch
            {
                "jpg" => new JpegEncoder()
                {
                    Quality = quality
                },
                "jpeg" => new JpegEncoder()
                {
                    Quality = quality
                },
                "png" => new PngEncoder(),                
                "gif" => new GifEncoder(),                
                "bmp" => new BmpEncoder(),                
                "webp" => new WebpEncoder()
                {
                    Quality = quality
                },
                "tga" => new TgaEncoder(),
                "tiff" => new TiffEncoder(),
                _ => throw new ImageProcessingException($"Unknown format '{preparedFormat}'")
            };
        }

    }
}
