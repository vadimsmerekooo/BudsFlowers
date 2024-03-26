using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            StatusMessage = "Сообщение успешно отправлено. Ожидайте ответа на указанную почту.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> SendReview(ReviewsViewModel model, string rate)
        {
            if (model.Review.FirstName.IsNullOrEmpty())
            {
                model.Review.FirstName = "Аноним";
            }
            model.Review.Status = TypeStatus.Опубликовано;
            model.Review.Type = TypeReview.All;
            model.Review.Star = Convert.ToInt32(rate);
            await _context.Reviews.AddAsync(model.Review);
            await _context.SaveChangesAsync();
            StatusMessage = "Отзыв успешно зарегестрирован. Спасибо, что Вы с нами!";
            return RedirectToAction(nameof(Reviews));
        }

        [Route("blog")]
        public async Task<IActionResult> Blog()
        {
            return View();
        }
        [Route("reviews")]
        public async Task<IActionResult> Reviews()
        {
            ReviewsViewModel model = new ReviewsViewModel()
            {
                Reviews = await _context.Reviews.Include(f => f.Flower).Where(r => r.Status == TypeStatus.Опубликовано && r.Type == TypeReview.All).ToListAsync()
            };
            if (User.Identity.IsAuthenticated)
            {
                User user = await _userManager.GetUserAsync(User);
                model.Review = new Review()
                {
                    Email = user.Email,
                    FirstName = user.UserName
                };
            }

            return View(model);
        }

        [HttpGet]
        [Route("order/info")]
        public async Task<IActionResult> OrderInfo(long orderNumber)
        {
            Order order = await _context.Orders.Include(f => f.Flowers).ThenInclude(p => p.Flower).ThenInclude(r => r.Reviews).Include(u => u.User).AsSplitQuery().FirstOrDefaultAsync(u => u.Number == orderNumber);
            if (order is null)
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