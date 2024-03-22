using BudsFlowers.Areas.Identity.Data;

namespace BudsFlowers.Models
{
    public class CarouselViewModel
    {
        public List<Flower> Flowers { get; set; }
        public TypeCarousel Type { get; set; }
    }
}
