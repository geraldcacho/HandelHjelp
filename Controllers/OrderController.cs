using HandleHjelp.Data;
using HandleHjelp.Data.Models;
using HandleHjelp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandleHjelp.Controllers
{
    public class OrderController : Controller
    {
        public readonly HandelhjelpContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(UserManager<ApplicationUser> userManager, HandelhjelpContext dbContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult Fulfilled()
        {
            return View();
        }

        public async Task <IActionResult> Create()
        {

            // Generating a timestamp
            int timeStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            // Ensure timestamp is not negative
            if (timeStamp < 0)
            {
                timeStamp = 0; // Set to 0 if negative
            }

            // Generating a random 6-digit number
            Random rnd = new Random();
            int randomPart = rnd.Next(100000, 999999); // Generates a random 6-digit number

            int uniqueId = (timeStamp * 1000000 + randomPart);

            // Ensure randomPart is not negative
            if (uniqueId < 0)
            {
                uniqueId *= -1; // Make positive if negative
            }

            var userId = _userManager.GetUserId(User);
            ViewBag.UserId = userId;

            // Combining the timestamp and random number

            return View(new EditOrderViewModel
            {
                IsNew = true,
                Order = new Data.Models.Order { OrderId = uniqueId.ToString() },
                Countries = await _dbContext.Countries.ToListAsync(),
                ProductTypes = await _dbContext.ProductTypes.ToListAsync(),
            });
        }

        public async Task<IActionResult> Edit(string orderId)
        {
            var order = await _dbContext.Orders.FirstAsync(self => self.OrderId == orderId);

            return View("Create", new EditOrderViewModel
            {
                IsNew = false,
                Order = order,
                Countries = await _dbContext.Countries.ToListAsync(),
                ProductTypes = await _dbContext.ProductTypes.ToListAsync(),
            });
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
