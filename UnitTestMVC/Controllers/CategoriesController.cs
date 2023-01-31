using Microsoft.AspNetCore.Mvc;

namespace UnitTestMVC.Controllers
{
    public class CategoriesController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
