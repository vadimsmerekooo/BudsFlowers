using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BudsFlowers.Controllers
{
    public class HomeController : Controller
    {
        private readonly BudsContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(BudsContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            List<Flower> allItem = await _context.Flowers.Include(r => r.Reviews).Where(f => f.IsInStock && f.TypeCategory == TypeCategory.Цветы).ToListAsync();
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
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                Message model = new Message()
                {
                    FirstName = user.UserName,
                    Email = user.Email
                };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message model)
        {
            await _context.Messages.AddAsync(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        [HttpGet]
        [Route("order/info")]
        public async Task<IActionResult> OrderInfo(long orderNumber)
        {
            Order order = await _context.Orders.Include(f => f.Flowers).ThenInclude(p => p.Flower).ThenInclude(r => r.Reviews).Include(u => u.User).AsSplitQuery().FirstOrDefaultAsync(u => u.Number == orderNumber);
            if(order is null)
            {
                StatusMessage = $"Ошибка Заказ #{orderNumber} не найден.";
                return RedirectToAction("Index");
            }
            return View(order);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}