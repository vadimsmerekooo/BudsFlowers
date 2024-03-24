using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using Elfie.Serialization;
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
        [Route("catalog/{id}/flowers/{page}")]
        public async Task<IActionResult> List(string id, int page = 1)
        {
            int pageSize = 10;

            CatalogViewModel model = new CatalogViewModel();
            model.Category = await _context.FlowerCategories.Include(f => f.Flowers).FirstOrDefaultAsync(c => c.Id == id);
            if (model.Category.Flowers.Count >= 1)
            {
                model.PriceMin = Convert.ToInt32(model.Category.Flowers.Min(c => c.GetTotalPrice()));
                model.PriceMax = Convert.ToInt32(model.Category.Flowers.Max(c => c.GetTotalPrice()));
            }
            model.Category.Flowers = model.Category.Flowers.Where(f => f.Status == TypeStatus.Опубликовано).OrderByDescending(p => p.IsPopular).ToList();

            var count = model.Category.Flowers.Count();
            var items = model.Category.Flowers.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel("Catalog", "List", id, count, page, pageSize);

            model.PageViewModel = pageViewModel;
            model.Category.Flowers = items;

            

            return View(model);
        }

        [Route("catalog/flowers/sort")]
        [Route("catalog/flowers/sort/{id}/{page}/{min}/{max}/{type}")]
        public async Task<IActionResult> Sort(string id, int page = 1, int min = 1, int max = 1, TypeSort type = TypeSort.По_умолчанию)
        {
            int pageSize = 10;

            CatalogViewModel model = new CatalogViewModel();
            model.Category = await _context.FlowerCategories.Include(f => f.Flowers).FirstOrDefaultAsync(c => c.Id == id);
            if (model.Category.Flowers.Count >= 1)
            {
                model.PriceMin = Convert.ToInt32(model.Category.Flowers.Min(c => c.GetTotalPrice()));
                model.PriceMax = Convert.ToInt32(model.Category.Flowers.Max(c => c.GetTotalPrice()));
            }
            model.Category.Flowers = model.Category.Flowers.Where(f => f.Status == TypeStatus.Опубликовано && f.GetTotalPrice() >= min && f.GetTotalPrice() <= max).ToList();

            var count = model.Category.Flowers.Count();
            switch (type)
            {
                case TypeSort.Сначала_дешевле:
                    model.Category.Flowers = model.Category.Flowers.OrderBy(p => p.GetTotalPrice()).ToList();
                    break;
                case TypeSort.Сначала_дороже:
                    model.Category.Flowers = model.Category.Flowers.OrderByDescending(p => p.GetTotalPrice()).ToList();
                    break;
                default:
                    model.Category.Flowers = model.Category.Flowers.OrderByDescending(p => p.IsPopular).ToList();
                    break;
            }
            var items = model.Category.Flowers.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel("Catalog", "Sort", id, count, min, max, type, page, pageSize);

            model.PageViewModel = pageViewModel;
            model.Category.Flowers = items;


            return PartialView("../Partials/_FlowersListPartial", model);

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
