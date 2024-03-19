using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class Basket
    {
        public long Code { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HomeNumber { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }

        public virtual List<BasketFlower> Flowers { get; set; }
    }
}
