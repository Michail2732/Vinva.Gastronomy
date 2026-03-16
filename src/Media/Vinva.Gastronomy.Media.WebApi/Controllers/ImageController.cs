using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Services;
using Vinva.Gastronomy.Media.Application.Usecases.CreateImage;
using Vinva.Gastronomy.Media.Application.Usecases.GetImagesByIds;
using Vinva.Gastronomy.Media.WebApi.Dto;

namespace Vinva.Gastronomy.Media.WebApi.Controllers
{
    [ApiController]
    [Route("api/Images")]
    [Authorize(Roles = UserRoles.Manager)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ImageController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserContext _userContext;

        public ImageController(IMediator mediator, IUserContext userContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }


        [HttpPost("GetByIds")]
        [Authorize(Roles = UserRoles.Client)]
        public async Task<GetImagesByIdsQueryResponse> GetImagesByIdsAsync([FromBody]GetImagesByIdsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10_500_000)]
        public async Task<CreateImageCommandResponse> CreateImageAsync([FromForm]CreateImageDto dto)
        {            
            var command = new CreateImageCommand
            {
                Content = dto.File.OpenReadStream(),
                ContentType = dto.File.ContentType,                
                FileName = dto.File.FileName,
                Group = dto.Group,
                OwnerId = _userContext.GetLogin(),
                Size = dto.File.Length
            };
            return await _mediator.Send(command);
        }

    }
}
