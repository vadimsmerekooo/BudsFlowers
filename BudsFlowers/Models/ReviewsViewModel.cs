using BudsFlowers.Areas.Identity.Data;

namespace BudsFlowers.Models
{
    public class ReviewsViewModel
    {
        public List<Review> Reviews { get; set; } = new List<Review>();
        public Review Review { get; set; } = new Review();
    }
}
