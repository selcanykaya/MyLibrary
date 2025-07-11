using System.Diagnostics;
using Kütüphanem.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kütüphanem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }


}

