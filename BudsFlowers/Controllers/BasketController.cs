using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using BudsFlowers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudsFlowers.Controllers
{
    public class BasketController : Controller
    {
        private readonly BudsContext _context;
        public BasketController(BudsContext context)
        {
            _context = context;
        }


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
                        flower.Flower = await _context.Flowers.Include(r => r.Reviews).FirstOrDefaultAsync(f => f.Id == flower.FlowerId);
                    }
                }

                BasketViewModel model = new BasketViewModel()
                {
                    Basket = basket,
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
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
                if(basket.Flowers is null)
                {
                    basket.Flowers = new List<BasketFlower>();
                }
                else
                {
                    foreach (var flower in basket.Flowers)
                    {
                        flower.Flower = await _context.Flowers.Include(r => r.Reviews).FirstOrDefaultAsync(f => f.Id == flower.FlowerId);
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
                if (Request.Cookies.ContainsKey("basket"))
                {
                    long code = long.Parse(Request.Cookies["basket"]);
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
                if(await BasketService.Delete(code, flowerId))
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
