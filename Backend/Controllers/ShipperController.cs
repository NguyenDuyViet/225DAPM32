using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class ShipperController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
