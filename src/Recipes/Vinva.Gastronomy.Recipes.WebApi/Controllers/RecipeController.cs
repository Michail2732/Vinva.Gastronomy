using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Create;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByFilter;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByIngredients;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Remove;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Update;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{    
    [ApiController]
    [Route("api/Recipes")]    
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RecipeController : Controller
    {
        private readonly IMediator _mediator;

        public RecipeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }        

        [HttpPost("Create")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<CreateRecipeResponce> Create([FromBody]CreateRecipeCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }        

        [HttpPost("SearchByIngredients")]
        [Authorize(Roles = UserRoles.Client)]
        public async Task<GetRecipeByIngredientsResponce> SearchByIngredients([FromBody]GetRecipeByIngredientsRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpPost("SearchByProperties")]
        [Authorize(Roles = UserRoles.Client)]        
        public async Task<GetRecipesByFilterQueryResponse> SearchByProperties([FromBody]GetRecipesByFilterQuery request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpDelete("Remove")]
        [Authorize(Roles = UserRoles.Administrator)]
        public async Task Remove([FromQuery] Guid recipeId)
        {
            var command = new RemoveCommand
            {
                RecipeId = recipeId
            };
            await _mediator.Send(command);            
        }

        [HttpPatch("Update")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task Update([FromBody]UpdateCommand command)
        {
            await _mediator.Send(command);            
        }       
    }
}
