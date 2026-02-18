using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Infrastructure.Results;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddIngredients;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddRecipeCategories;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.AddSteps;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.CreateRecipe;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetRecipeByCategory;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.GetRecipeByIngredients;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveIngredients;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveRecipeCategory;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.RemoveSteps;
using Vinva.Gastronomy.Recipes.Application.RecipeUsecases.ReorderSteps;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{
    [Route("api/Recipes")]
    [Authorize(Roles = UserRoles.Administrator)]
    public class RecipeController : Controller
    {
        private readonly IMediator _mediator;

        public RecipeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPatch]
        public async Task<Result> AddIngredients([FromBody]AddIngredientCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPatch]
        public async Task<Result> AddCategories([FromBody]AddRecipeCategoriesCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPatch]
        public async Task<Result> AddSteps([FromBody]AddStepsCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPost]
        public async Task<Result<CreateRecipeResponce>> Create([FromBody]CreateRecipeCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        public async Task<Result<GetRecipeByCategoryResponce>> GetByCategories([FromBody]GetRecipeByCategoryRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.User)]
        public async Task<Result<GetRecipeByIngredientsResponce>> GetByIngredients([FromBody]GetRecipeByIngredientsRequest request)
        {
            var result = await _mediator.Send(request);
            return result;
        }

        [HttpPatch]
        public async Task<Result> RemoveIngredients([FromBody]RemoveIngredientsCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPatch]
        public async Task<Result> RemoveCategories([FromBody]RemoveRecipeCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPatch]
        public async Task<Result> RemoveSteps([FromBody]RemoveStepsCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPatch]
        public async Task<Result> ReorderSteps([FromBody]ReorderStepsCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }        
    }
}
