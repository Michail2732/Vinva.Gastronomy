using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Create;
using Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.GetById;
using Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Remove;
using Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Update;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{
    [ApiController]
    [Route("api/Ingredients")]
    [Authorize(Roles = UserRoles.Manager)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class IngredientController : Controller
    {
        private readonly IMediator _mediator;

        public IngredientController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("Create")]
        public async Task<CreateIngredientResponse> Create([FromBody]CreateIngredientCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }


        [HttpGet("GetById")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<GetIngredientByIdResponce> GetById([FromQuery] Guid ingredientId)
        {
            var request = new GetIngredientByIdRequest
            {
                IngredientId = ingredientId
            };
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpDelete("Remove")]
        [Authorize(Roles = UserRoles.Administrator)]
        public async Task Remove([FromBody] RemoveIngredientCommand command)
        {
            await _mediator.Send(command);            
        }

        [HttpPatch("Update")]
        public async Task Update([FromBody] UpdateIngredientCommand command)
        {
            await _mediator.Send(command);            
        }

    }
}
