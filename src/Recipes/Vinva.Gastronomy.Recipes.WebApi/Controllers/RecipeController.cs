using Microsoft.AspNetCore.Mvc;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{
    public class RecipeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
