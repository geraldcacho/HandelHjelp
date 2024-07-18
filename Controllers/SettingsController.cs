using Microsoft.AspNetCore.Mvc;

namespace HandleHjelp.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
