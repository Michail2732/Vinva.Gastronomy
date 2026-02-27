using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddCategories;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddIngredients;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.AddSteps;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.Create;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByCategory;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByFilter;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.GetByIngredients;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveCategory;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveIngredients;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.RemoveSteps;
using Vinva.Gastronomy.Recipes.Application.Usecases.Recipes.ReorderSteps;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{    
    [ApiController]
    [Route("api/Recipes")]
    [Authorize(Roles = UserRoles.Administrator)]
    public class RecipeController : Controller
    {
        private readonly IMediator _mediator;

        public RecipeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPatch("AddIngredients")]
        public async Task<Result> AddIngredients([FromBody]AddIngredientCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPatch("AddCategories")]
        public async Task<Result> AddCategories([FromBody]AddRecipeCategoriesCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPatch("AddSteps")]
        public async Task<Result> AddSteps([FromBody]AddStepsCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPost("Create")]
        public async Task<Result<CreateRecipeResponce>> Create([FromBody]CreateRecipeCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPost("SearchByCategories")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<Result<GetRecipeByCategoryResponce>> SearchByCategories([FromBody]GetRecipeByCategoryRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpPost("SearchByIngredients")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<Result<GetRecipeByIngredientsResponce>> SearchByIngredients([FromBody]GetRecipeByIngredientsRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpPost("SearchByQuery")]
        [Authorize(Roles = UserRoles.User)]
        [AllowAnonymous]
        public async Task<Result<GetRecipesByFilterQueryResponse>> SearchByQuery([FromBody]GetRecipesByFilterQuery request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpPatch("RemoveIngredients")]        
        public async Task<Result> RemoveIngredients([FromBody]RemoveIngredientsCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPatch("RemoveCategories")]
        public async Task<Result> RemoveCategories([FromBody]RemoveRecipeCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPatch("RemoveSteps")]
        public async Task<Result> RemoveSteps([FromBody]RemoveStepsCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPatch("ReorderSteps")]
        public async Task<Result> ReorderSteps([FromBody]ReorderStepsCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }        
    }
}
