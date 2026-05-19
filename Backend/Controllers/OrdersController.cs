using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
