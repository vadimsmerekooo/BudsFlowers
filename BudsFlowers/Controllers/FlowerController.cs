using BudsFlowers.Areas.Identity.Data;
using BudsFlowers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BudsFlowers.Controllers
{
    public class FlowerController : Controller
    {
        private readonly BudsContext _context;
        private readonly UserManager<User> _userManager;
        public FlowerController(BudsContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        [HttpPost]
        public async Task<IActionResult> SendReview(FlowerViewModel model, string rate)
        {
            model.Review.Star = Convert.ToInt32(rate);
            model.Review.Flower = await _context.Flowers.FirstOrDefaultAsync(f => f.Id == model.Flower.Id);
            if (User.Identity.IsAuthenticated)
            {
                model.Review.User = await _userManager.GetUserAsync(User);
                model.Review.FirstName = model.Review.User.UserName;
                model.Review.Email = model.Review.User.Email;
            }
            else
            {
                if (model.Review.FirstName.IsNullOrEmpty())
                    model.Review.FirstName = "Аноним";
            }
            await _context.Reviews.AddAsync(model.Review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = model.Flower.Id } );
        }
    }
}
