using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.WebApi.Services;

namespace Vinva.Gastronomy.Media.Application.Services
{
    public class ImageFormatService : IImageFormatService
    {
        public Task<string> GetFormatAsync(Stream imageStream, string contentType, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Validate(Stream imageStream, string contentType, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
