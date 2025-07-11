using Microsoft.AspNetCore.Mvc;

namespace Kütüphanem.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
