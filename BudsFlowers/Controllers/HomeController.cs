using BudsFlowers.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BudsFlowers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("catalog")]
        public async Task<IActionResult> Catalog()
        {
            return View();
        }
        [Route("delivery-pay")]
        public async Task<IActionResult> Delivery()
        {
            return View();
        }
        [Route("about")]
        public async Task<IActionResult> About()
        {
            return View();
        }
        [Route("blog")]
        public async Task<IActionResult> Blog()
        {
            return View();
        }
        [Route("reviews")]
        public async Task<IActionResult> Reviews()
        {
            return View();
        }

        public void AddBasket()
        {

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}