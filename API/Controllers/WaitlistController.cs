using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class WaitlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
