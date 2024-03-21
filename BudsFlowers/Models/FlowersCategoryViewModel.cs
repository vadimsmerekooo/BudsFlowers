using BudsFlowers.Areas.Identity.Data;

namespace BudsFlowers.Models
{
    public class FlowersCategoryViewModel
    {
        public List<Flower> Flowers { get; set; }
        public TypeCategory TypeCategory { get; set; }
    }
}
