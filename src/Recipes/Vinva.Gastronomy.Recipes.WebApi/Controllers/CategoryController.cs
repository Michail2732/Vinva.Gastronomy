using Microsoft.AspNetCore.Mvc;

namespace Vinva.Gastronomy.Recipes.WebApi.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
