using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Exceptions;
using Vinva.Gastronomy.Media.WebApi.Services;

namespace Vinva.Gastronomy.Media.Application.Services
{
    public class ImageFormatService : IImageFormatService
    {
        private static readonly IReadOnlyDictionary<string, string> _formatsByContentType =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["image/jpeg"] = "jpeg",
                ["image/png"] = "png",
                ["image/gif"] = "gif",
                ["image/webp"] = "webp",
                ["image/bmp"] = "bmp",
                ["image/avif"] = "avif",
                ["image/tiff"] = "tiff",
                ["image/svg+xml"] = "svg",
            };

        public Task<string> GetFormatAsync(Stream imageStream, string contentType, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(contentType) || !_formatsByContentType.TryGetValue(contentType, out var format))
                throw new BadRequestException($"Неподдерживаемый тип изображения '{contentType}'");

            return Task.FromResult(format);
        }

        public Task<bool> Validate(Stream imageStream, string contentType, CancellationToken ct = default)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(contentType) && _formatsByContentType.ContainsKey(contentType));
        }
    }
}
