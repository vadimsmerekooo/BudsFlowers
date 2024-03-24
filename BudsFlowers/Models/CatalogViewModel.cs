using BudsFlowers.Areas.Identity.Data;

namespace BudsFlowers.Models
{
    public class CatalogViewModel
    {
        public PageViewModel PageViewModel { get; set; }
        public FlowerCategory Category { get; set; }
        public int PriceMin { get; set; }
        public int PriceMax { get; set; }
    }
}
