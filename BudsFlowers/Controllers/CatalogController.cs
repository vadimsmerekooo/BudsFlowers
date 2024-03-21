using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudsFlowers.Controllers
{
    public class CatalogController : Controller
    {
        private readonly BudsContext _context;
        public CatalogController(BudsContext context)
        {
            _context = context;
        }

        [Route("catalog")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.FlowerCategories.Where(c => c.TypeStatus == TypeStatus.Опубликовано).ToListAsync());
        }

        [Route("catalog/{id}/flowers")]
        public async Task<IActionResult> List(string id)
        {
            CatalogViewModel model = new CatalogViewModel();
            model.Category = await _context.FlowerCategories.Include(f => f.Flowers).FirstOrDefaultAsync(c => c.Id == id);
            model.PriceMin = Convert.ToInt32(model.Category.Flowers.Min(c => c.GetTotalPrice()));
            model.PriceMax = Convert.ToInt32(model.Category.Flowers.Max(c => c.GetTotalPrice()));
            return View(model);
        }
    }
}
