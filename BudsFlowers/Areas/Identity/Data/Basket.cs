using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class Basket
    {
        public long Code { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Почта")]
        public string Email { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "Город")]
        public string City { get; set; } = "Гродно";
        [Display(Name = "Улица")]
        public string Street { get; set; }
        [Display(Name = "Дом")]
        public string HomeNumber { get; set; }
        [Display(Name = "Дата доставки")]
        public string DeliveryDate { get; set; }
        [Display(Name = "Время доставки")]
        public string DeliveryTime { get; set; }

        public List<BasketFlower> Flowers { get; set; } = new List<BasketFlower>();

        public double GetTotalPrice() => Flowers.Sum(f => f.GetTotalPrice());
        public double GetPrice() => Flowers.Sum(f => f.GetPrice());
    }
}
