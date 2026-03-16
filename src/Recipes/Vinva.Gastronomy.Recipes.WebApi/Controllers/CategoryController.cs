using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Application.Usecases.Categories.CreateCategory;
using Vinva.Gastronomy.Recipes.Application.Usecases.Categories.RemoveCategory;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{
    [ApiController]
    [Route("api/Categories")]
    [Authorize(Roles = UserRoles.Manager)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class CategoryController : Controller
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("Create")]
        public async Task<CreateCategoryResponse> Create([FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpDelete("Remove")]
        [Authorize(Roles = UserRoles.Administrator)]
        public async Task Remove([FromBody] RemoveCategoryCommand command)
        {
            await _mediator.Send(command);            
        }
    }
}
