using BudsFlowers.Areas.Identity.Data;

namespace BudsFlowers.Models
{
    public class FlowerViewModel
    {
        public Flower Flower { get; set; }
        public List<Flower> Other { get; set; }
        public Review Review { get; set; }
    }
}
