using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Application.Usecases.Create;
using Vinva.Gastronomy.Recipes.Application.Usecases.SearchByCharacteristics;
using Vinva.Gastronomy.Recipes.Application.Usecases.Update;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Remove;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{
    [ApiController]
    [Route("api/recipes")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public sealed class RecipeController : ControllerBase
    {
        private readonly IMediator mediator;

        public RecipeController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        [ProducesResponseType(typeof(CreateRecipeResponce), StatusCodes.Status201Created)]
        public async Task<ActionResult<CreateRecipeResponce>> Create(
            [FromBody] CreateRecipeCommand command,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPatch("{id:guid}")]
        [Authorize(Roles = UserRoles.Manager)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateCommand command,
            CancellationToken cancellationToken)
        {
            if (command.Id != id)
                return BadRequest("The route id and command id must match.");

            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = UserRoles.Administrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoveCommand { RecipeId = id }, cancellationToken);
            return NoContent();
        }

        [HttpPost("search-by-characteristics")]
        [Authorize(Roles = UserRoles.Client)]
        [ProducesResponseType(typeof(SearchRecipesByCharacteristicsResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<SearchRecipesByCharacteristicsResponse>> Search(
            [FromBody] SearchRecipesByCharacteristicsQuery query,
            CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(query, cancellationToken));
        }
    }
}
