using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
