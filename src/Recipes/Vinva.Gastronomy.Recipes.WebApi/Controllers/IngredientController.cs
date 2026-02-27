using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Application.Usecases.Ingredients.Create;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{
    [ApiController]
    [Route("api/Ingredients")]
    [Authorize(Roles = UserRoles.Administrator)]
    public class IngredientController : Controller
    {
        private readonly IMediator _mediator;

        public IngredientController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPatch("AddIngredients")]
        public async Task<CreateIngredientResponse> Create([FromBody]CreateIngredientCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }


    }
}
