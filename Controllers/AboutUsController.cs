using Microsoft.AspNetCore.Mvc;

namespace HandleHjelp.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult TermsAndAggreements()
        {
            return View();
        }

        public IActionResult FAQS()
        {
            return View();
        }
    }
}
