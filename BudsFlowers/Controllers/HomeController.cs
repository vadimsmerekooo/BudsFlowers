using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BudsFlowers.Controllers
{
    public class HomeController : Controller
    {
        private readonly BudsContext _context;

        public HomeController(BudsContext context)
        {
            _context = context;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            List<Flower> allItem = await _context.Flowers.Include(r => r.Reviews).ToListAsync();
            List<Flower> popularFlower = allItem.Where(p => p.IsPopular && !p.IsSale()).Take(10).ToList();
            popularFlower.ForEach(f => allItem.Remove(f));
            List<Flower> saleFlower = allItem.Where(s => s.IsSale()).Take(10).ToList();
            saleFlower.ForEach(f => allItem.Remove(f));
            List<Flower> newFlower = allItem.OrderByDescending(d => d.Create).Take(10).ToList();
            newFlower.ForEach(f => allItem.Remove(f));



            IndexViewModel model = new IndexViewModel()
            {
                PopularCarousel = new CarouselViewModel() { Type = TypeCarousel.Popular, Flowers = popularFlower },
                NewCarousel = new CarouselViewModel() { Type = TypeCarousel.Popular, Flowers = newFlower },
                SaleCarousel = new CarouselViewModel() { Type = TypeCarousel.Popular, Flowers = saleFlower }
            };
            return View(model);
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