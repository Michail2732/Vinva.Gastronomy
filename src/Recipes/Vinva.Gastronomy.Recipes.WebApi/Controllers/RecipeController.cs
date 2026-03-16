using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddCategories;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddIngredients;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddSteps;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Create;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByCategory;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByFilter;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByIngredients;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Remove;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveCategory;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveIngredients;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveSteps;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.ReorderSteps;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{    
    [ApiController]
    [Route("api/Recipes")]
    [Authorize(Roles = UserRoles.Manager)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RecipeController : Controller
    {
        private readonly IMediator _mediator;

        public RecipeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPatch("AddIngredients")]
        public async Task AddIngredients([FromBody]AddIngredientCommand command)
        {
            await _mediator.Send(command);            
        }

        [HttpPatch("AddCategories")]
        public async Task AddCategories([FromBody]AddRecipeCategoriesCommand command)
        {
            await _mediator.Send(command);            
        }

        [HttpPatch("AddSteps")]
        public async Task AddSteps([FromBody]AddStepsCommand command)
        {
            await _mediator.Send(command);            
        }

        [HttpPost("Create")]
        public async Task<CreateRecipeResponce> Create([FromBody]CreateRecipeCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPost("SearchByCategories")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<GetRecipeByCategoryResponce> SearchByCategories([FromBody]GetRecipeByCategoryRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpPost("SearchByIngredients")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<GetRecipeByIngredientsResponce> SearchByIngredients([FromBody]GetRecipeByIngredientsRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpPost("SearchByQuery")]
        [Authorize(Roles = UserRoles.User)]        
        public async Task<GetRecipesByFilterQueryResponse> SearchByQuery([FromBody]GetRecipesByFilterQuery request)
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

        [HttpPatch("RemoveIngredients")]        
        public async Task RemoveIngredients([FromBody]RemoveIngredientsCommand command)
        {
            await _mediator.Send(command);            
        }

        [HttpPatch("RemoveCategories")]
        public async Task RemoveCategories([FromBody]RemoveRecipeCategoryCommand command)
        {
            await _mediator.Send(command);            
        }

        [HttpPatch("RemoveSteps")]
        public async Task RemoveSteps([FromBody]RemoveStepsCommand command)
        {
            await _mediator.Send(command);            
        }

        [HttpPatch("ReorderSteps")]
        public async Task ReorderSteps([FromBody]ReorderStepsCommand command)
        {
            await _mediator.Send(command);            
        }        
    }
}
