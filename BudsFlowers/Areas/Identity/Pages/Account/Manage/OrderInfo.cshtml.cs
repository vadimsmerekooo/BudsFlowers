#nullable disable

using BudsFlowers.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BudsFlowers.Areas.Identity.Pages.Account.Manage
{
    public class OrderInfoModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly BudsContext _context;
        public Order Order { get; set; }

        public OrderInfoModel(
            UserManager<User> userManager,
            BudsContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            Order = await _context.Orders.Include(f => f.Flowers).ThenInclude(p => p.Flower).ThenInclude(r => r.Reviews).Include(u => u.User).AsSplitQuery().FirstOrDefaultAsync(u => u.User.Id == user.Id && u.Id == id);
            if(Order == null)
            {
                StatusMessage = "Ошибка Заказ не найден!";
                return RedirectToPage("Orders");
            }

            return Page();
        }
    }
}
