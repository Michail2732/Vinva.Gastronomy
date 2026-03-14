using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Media.Domain.Entities;
using Vinva.Gastronomy.Media.Domain.Models;

namespace Vinva.Gastronomy.Media.Application.Services
{
    public interface IImagesService
    {        
        Task<string> GetProcessedUrlAsync(ImageMetadata item, ImageOptions conversionOptions, CancellationToken ct = default);
    }
}
