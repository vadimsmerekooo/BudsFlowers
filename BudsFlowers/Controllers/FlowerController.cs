using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudsFlowers.Controllers
{
    public class FlowerController : Controller
    {
        private readonly BudsContext _context;
        public FlowerController(BudsContext context)
        {
            _context = context;
        }

        [Route("catalog/flower/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            FlowerViewModel model = new FlowerViewModel();
            model.Flower = await _context.Flowers.Include(r => r.Reviews).Include(c => c.Category).FirstOrDefaultAsync(f => f.Id == id);
            model.Other = await _context.Flowers
                .Where(s => s.Status == TypeStatus.Опубликовано 
                && s.IsInStock 
                && (s.TypeCategory == TypeCategory.Игрушки 
                || s.TypeCategory == TypeCategory.Конфеты))
                .Take(6)
                .ToListAsync();
            return View(model);
        }

    }
}
