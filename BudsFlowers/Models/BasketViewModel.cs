using BudsFlowers.Areas.Identity.Data;

namespace BudsFlowers.Models
{
    public class BasketViewModel
    {
        public Basket Basket { get; set; }
        public double TotalPrice { get; set; }
        public int Delivery { get; set; }
    }
}
