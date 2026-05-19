using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
