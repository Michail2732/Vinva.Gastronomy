using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Media.WebApi.Dto
{
    public record CreateImageDto
    {
        public required IFormFile File { get; init; }
        public string? Group { get; init; }
    }
}
