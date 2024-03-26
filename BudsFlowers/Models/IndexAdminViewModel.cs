using BudsFlowers.Areas.Identity.Data;

namespace BudsFlowers.Models
{
    public class IndexAdminViewModel
    {
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
