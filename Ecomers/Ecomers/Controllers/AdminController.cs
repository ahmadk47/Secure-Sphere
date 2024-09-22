using Microsoft.AspNetCore.Mvc;

namespace Ecomers.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
