using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
