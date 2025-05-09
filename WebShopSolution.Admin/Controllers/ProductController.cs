using Microsoft.AspNetCore.Mvc;

namespace WebShopSolution.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
