using Microsoft.AspNetCore.Mvc;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{
    public class IngredientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
