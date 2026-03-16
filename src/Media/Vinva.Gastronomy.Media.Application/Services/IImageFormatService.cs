using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Media.WebApi.Services
{
    public interface IImageFormatService
    {
        Task<string> GetFormatAsync(Stream imageStream, string contentType, CancellationToken ct = default);
        Task<bool> Validate(Stream imageStream, string contentType, CancellationToken ct = default);
    }
}
