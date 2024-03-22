using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using BudsFlowers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudsFlowers.Controllers
{
    public class BasketController : Controller
    {
        private readonly BudsContext _context;
        public BasketController(BudsContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            if (Request.Cookies.ContainsKey("basket"))
            {
                long code = long.Parse(Request.Cookies["basket"]);
                Basket basket = BasketService.GetBasketByCode(code);
                if(basket is null)
                {
                    return RedirectToAction("Index", "Home");
                }
                basket.Flowers = new List<BasketFlower>();

                BasketViewModel model = new BasketViewModel()
                {
                    Basket = basket,
                    Delivery = 2,
                    TotalPrice = 3123
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Route("basket/add/{flowerId}/{count}")]
        public IActionResult Add(string flowerId, int count = 1)
        {
            try
            {
                if (Request.Cookies.ContainsKey("basket"))
                {
                    long code = long.Parse(Request.Cookies["basket"]);
                    if(_context.Flowers.Any(c => c.Id == flowerId))
                    {
                        BasketFlower flower = new BasketFlower()
                        {
                            Count = count,
                            FlowerId = flowerId
                        };
                        if(BasketService.AddItem(code, flower))
                        {
                            return Json(new { success = true }, new Newtonsoft.Json.JsonSerializerSettings());
                        }
                    }

                }
                return Json(new { success = false }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch
            {
                return Json(new { success = false }, new Newtonsoft.Json.JsonSerializerSettings());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
