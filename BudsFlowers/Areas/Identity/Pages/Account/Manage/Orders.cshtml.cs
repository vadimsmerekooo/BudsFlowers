#nullable disable

using BudsFlowers.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BudsFlowers.Areas.Identity.Pages.Account.Manage
{
    public class OrdersModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly BudsContext _context;
        public List<Order> Orders { get; set; } = new List<Order>();

        public OrdersModel(
            UserManager<User> userManager,
            BudsContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> LoadAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Orders = _context.Orders.Include(f => f.Flowers).Include(u => u.User).Where(u => u.User.Id == user.Id).ToList();

            return Page();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
