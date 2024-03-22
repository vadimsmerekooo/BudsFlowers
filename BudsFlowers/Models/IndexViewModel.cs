namespace BudsFlowers.Models
{
    public class IndexViewModel
    {
        public CarouselViewModel PopularCarousel { get; set; } = new CarouselViewModel();
        public CarouselViewModel NewCarousel { get; set; } = new CarouselViewModel();
        public CarouselViewModel SaleCarousel { get; set; } = new CarouselViewModel();
    }
}
