using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using BudsFlowers.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;

namespace BudsFlowers.Controllers
{
    public class BasketController : Controller
    {
        private readonly BudsContext _context;
        private readonly UserManager<User> _userManager;

        public BasketController(BudsContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }


        [Route("basket")]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies.ContainsKey("basket"))
            {
                long code = long.Parse(Request.Cookies["basket"]);
                Basket basket = BasketService.GetBasketByCode(code);
                if (basket is null)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (basket.Flowers is null)
                {
                    basket.Flowers = new List<BasketFlower>();
                }
                else
                {
                    foreach (var flower in basket.Flowers)
                    {
                        Flower flowerConext = await _context.Flowers.Include(r => r.Reviews).FirstOrDefaultAsync(f => f.Id == flower.FlowerId);

                        flower.Flower = flowerConext;
                    }
                }

                BasketViewModel model = new BasketViewModel()
                {
                    Basket = basket,
                };
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.GetUserAsync(User);
                    model.Basket.FirstName = user.UserName;
                    model.Basket.Email = user.Email;
                    model.Basket.Phone = user.PhoneNumber;
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> OrderCreate(BasketViewModel model)
        {
            if (Request.Cookies.ContainsKey("basket"))
            {
                long code = long.Parse(Request.Cookies["basket"]);
                long number = new Random().Next(1, 999999999);
                DateTime dateTimeNow = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime dateDelivery = Convert.ToDateTime(model.Basket.DeliveryDate);

                if(dateDelivery.Date <= DateTime.Now.Date)
                {
                    StatusMessage = "Ошибка Некорректно выбрана дата. Оформление заказа возможно за одни сутки до доставки курьером.";
                    return RedirectToAction(nameof(Index), model);
                }
                Order order = new Order()
                {
                    FirstName = model.Basket.FirstName,
                    Email = model.Basket.Email,
                    Phone = model.Basket.Phone,
                    City = model.Basket.City,
                    Street = model.Basket.Street,
                    HomeNumber = model.Basket.HomeNumber,
                    Code = model.Basket.Code,
                    DeliveryDate = model.Basket.DeliveryDate,
                    DeliveryTime = model.Basket.DeliveryTime,
                    Number = number
                };
                var user = await _userManager.FindByEmailAsync(order.Email);
                if (user != null)
                {
                    order.User = user;
                }

                List<BasketFlower> flowers = BasketService.GetBasketFlower(code);

                foreach (var flower in flowers)
                {
                    Flower flowerContext = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == flower.FlowerId);
                    if(flowerContext.IsInStock && flowerContext.Status == TypeStatus.Опубликовано)
                    {
                        order.Flowers.Add(new OrderItem()
                        {
                            Flower = flowerContext,
                            Count = flower.Count,
                            Price = flowerContext.Price,
                            Sale = flowerContext.GetTotalPrice()
                        });
                        flowerContext.Orders = flowerContext.Orders + 1;
                        _context.Flowers.Update(flowerContext);
                        await _context.SaveChangesAsync();
                    }
                }
                order.TotalPrice = order.Flowers.Sum(s => s.GetPrice());
                order.TotalSale = order.Flowers.Sum(s => s.GetTotalPrice());

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                Clear();
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(order.Email.Trim(), $"Заказ #{number}", $"Заказ #{number} на сумму {order.TotalSale} руб., успешно оформлен. Отслеживайте статус заказ по ссылке: {Url.ActionLink("OrderInfo", "Home", new { number = number })}"); ;

                StatusMessage = $"Заказ #{number} на сумму {order.TotalSale} руб., успешно оформлен. Отслеживайте статус заказ по ссылке: <a href=\"{Url.ActionLink("OrderInfo", "Home", new { number = number })}\">#{number}</a>";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                StatusMessage = "Ошибка Заказ не оформлен. Побробуйте еще раз!";
                return RedirectToAction(nameof(Index), model);
            }
        }

        [HttpGet]
        [Route("basket/basket-update")]
        public async Task<IActionResult> IndexPartial()
        {
            if (Request.Cookies.ContainsKey("basket"))
            {
                long code = long.Parse(Request.Cookies["basket"]);
                Basket basket = BasketService.GetBasketByCode(code);
                if (basket is null)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (basket.Flowers is null)
                {
                    basket.Flowers = new List<BasketFlower>();
                }
                else
                {
                    foreach (var flower in basket.Flowers)
                    {
                        Flower flowerConext = await _context.Flowers.Include(r => r.Reviews).FirstOrDefaultAsync(f => f.Id == flower.FlowerId);
                        flower.Flower = flowerConext;
                    }
                }

                BasketViewModel model = new BasketViewModel()
                {
                    Basket = basket
                };

                return PartialView("../Partials/_BasketIndexPartial", model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Route("basket/add/{flowerId}/{count}")]
        public async Task<IActionResult> Add(string flowerId, int count = 1)
        {
            try
            {
                long code;
                if (Request.Cookies.ContainsKey("basket") && long.TryParse(Request.Cookies["basket"], out code))
                {
                    if (_context.Flowers.Any(c => c.Id == flowerId))
                    {
                        BasketFlower flower = new BasketFlower()
                        {
                            Count = count,
                            FlowerId = flowerId
                        };
                        if (await BasketService.AddItem(code, flower))
                        {
                            return Json(new { success = true, error = false });
                        }
                    }

                }
                else
                {

                    code = BasketService.SetId();
                    if (code != 0)
                    {
                        this.Response.Cookies.Delete("basket");
                        this.Response.Cookies.Append("basket", code.ToString());
                        if (_context.Flowers.Any(c => c.Id == flowerId))
                        {
                            BasketFlower flower = new BasketFlower()
                            {
                                Count = count,
                                FlowerId = flowerId
                            };
                            if (await BasketService.AddItem(code, flower))
                            {
                                return Json(new { success = true, error = false });
                            }
                        }
                    }
                }
                return Json(new { success = false, error = true });
            }
            catch
            {
                return Json(new { success = false, error = true });
            }
        }
        [HttpGet]
        [Route("basket/plus/{flowerId}")]
        public async Task<IActionResult> Plus(string flowerId)
        {
            try
            {
                if (Request.Cookies.ContainsKey("basket"))
                {
                    long code = long.Parse(Request.Cookies["basket"]);
                    if (_context.Flowers.Any(c => c.Id == flowerId))
                    {
                        if (await BasketService.PlusItem(code, flowerId))
                        {
                            return Json(new { success = true, error = false });
                        }
                    }

                }
                return Json(new { success = false, error = true });
            }
            catch
            {
                return Json(new { success = false, error = true });
            }
        }
        [HttpGet]
        [Route("basket/minus/{flowerId}")]
        public async Task<IActionResult> Minus(string flowerId)
        {
            try
            {
                if (Request.Cookies.ContainsKey("basket"))
                {
                    long code = long.Parse(Request.Cookies["basket"]);
                    if (_context.Flowers.Any(c => c.Id == flowerId))
                    {
                        if (await BasketService.MinusItem(code, flowerId))
                        {
                            return Json(new { success = true, error = false });
                        }
                    }

                }
                return Json(new { success = false, error = true });
            }
            catch
            {
                return Json(new { success = false, error = true });
            }
        }


        public async Task<int> GetBasketCount()
        {
            if (Request.Cookies.ContainsKey("basket"))
            {
                long code = long.Parse(Request.Cookies["basket"]);
                return BasketService.GetBasketCount(code);
            }
            return 0;
        }

        [HttpGet]
        [Route("basket/delete/{flowerId}")]
        public async Task<IActionResult> Delete(string flowerId)
        {
            if (Request.Cookies.ContainsKey("basket"))
            {
                long code = long.Parse(Request.Cookies["basket"]);
                if (await BasketService.Delete(code, flowerId))
                {
                    return Json(new { success = true, error = false });
                }
                else
                {
                    return Json(new { success = false, error = true });
                }
            }
            return Json(new { success = false, error = true });
        }
        [HttpGet]
        [Route("basket/clear")]
        public async Task<IActionResult> Clear()
        {
            if (Request.Cookies.ContainsKey("basket"))
            {
                long code = long.Parse(Request.Cookies["basket"]);
                if (await BasketService.ClearBasket(code))
                {
                    return Json(new { success = true, error = false });
                }
                else
                {
                    return Json(new { success = false, error = true });
                }
            }
            return Json(new { success = false, error = true });
        }
    }
}
