using Microsoft.AspNetCore.Mvc;

namespace HandleHjelp.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
