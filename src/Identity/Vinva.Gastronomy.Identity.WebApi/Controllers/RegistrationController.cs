using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vinva.Gastronomy.Identity.Application.Usecases.Registration.Register;
using Vinva.Gastronomy.Identity.Application.Usecases.Registration.RegisterConfirm;

namespace Vinva.Gastronomy.Identity.WebApi.Controllers
{
    [ApiController]
    [Route("api/Registration")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegistrationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task Register([FromBody]RegisterCommand command)
        {
            await _mediator.Send(command);
        }

        [HttpPost("RegisterConfirm")]
        [AllowAnonymous]
        public async Task RegisterConfirm([FromQuery]Guid tokenId)
        {
            var command = new RegisterConfirmCommand
            {
                TokenId = tokenId
            };
            await _mediator.Send(command);
        }

    }
}
