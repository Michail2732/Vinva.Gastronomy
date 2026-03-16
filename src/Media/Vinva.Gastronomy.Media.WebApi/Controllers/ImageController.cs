using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Media.Application.Usecases.CreateImage;
using Vinva.Gastronomy.Media.Application.Usecases.GetImagesByIds;

namespace Vinva.Gastronomy.Media.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoles.Manager)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImageController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        [HttpPost("GetByIds")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<GetImagesByIdsQueryResponse> GetImagesByIdsAsync([FromBody]GetImagesByIdsQuery query)
        {

        }

        [HttpPost("Create")]
        [RequestSizeLimit(10_500_000)]
        public async Task<CreateImageCommandResponse> CreateImageAsync(IFormFile file)
        {
            var dto = new CreateImageCommand
            {
                Content = file.OpenReadStream(),
                Format = file.ContentType,
                Name = file.Name,
                
            }
        }

    }
}
