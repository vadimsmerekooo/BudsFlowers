using BudsFlowers.Areas.Identity.Data;
using static NuGet.Packaging.PackagingConstants;

namespace BudsFlowers.Models
{
    public class BasketViewModel
    {
        public Basket Basket { get; set; }
        public int Delivery { get; set; }
    }
}
