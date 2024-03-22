using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;

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
            return View(await _context.FlowerCategories.Where(c => c.TypeStatus == TypeStatus.Опубликовано).OrderBy(t => t.TypeCategory).ToListAsync());
        }

        [Route("catalog/{id}/flowers")]
        public async Task<IActionResult> List(string id)
        {
            CatalogViewModel model = new CatalogViewModel();
            model.Category = await _context.FlowerCategories.Include(f => f.Flowers).FirstOrDefaultAsync(c => c.Id == id);
            if(model.Category.Flowers.Count >= 1)
            {
                model.PriceMin = Convert.ToInt32(model.Category.Flowers.Min(c => c.GetTotalPrice()));
                model.PriceMax = Convert.ToInt32(model.Category.Flowers.Max(c => c.GetTotalPrice()));
            }

            return View(model);
        }

        [Route("catalog/search/{query}")]
        public async Task<IActionResult> Search(string query)
        {
            try
            {
                List<Flower> flowers = await _context.Flowers.Where(f => f.Title.Contains(query)).Take(5).ToListAsync();
                return PartialView("../Partials/_SearchResultPartial", flowers);
            }
            catch
            {
                return PartialView("../Partials/_SearchResultPartial", new List<Flower>());
            }
        }
    }
}
