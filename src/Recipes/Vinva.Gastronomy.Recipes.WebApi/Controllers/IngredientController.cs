using Microsoft.AspNetCore.Mvc;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{
    [Route("api/Ingredients")]
    public class IngredientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
