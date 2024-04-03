using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class Basket
    {
        public long Code { get; set; }
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Заполните поле.")]
        public string FirstName { get; set; }
        [Display(Name = "Почта")]
        [Required(ErrorMessage = "Заполните поле.")]
        public string Email { get; set; }
        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Заполните поле.")]
        public string Phone { get; set; }
        [Display(Name = "Город")]
        [Required(ErrorMessage = "Заполните поле.")]
        public string City { get; set; } = "Гродно";
        [Display(Name = "Улица")]
        [Required(ErrorMessage = "Заполните поле.")]
        public string Street { get; set; }
        [Display(Name = "Дом")]
        [Required(ErrorMessage = "Заполните поле.")]
        public string HomeNumber { get; set; }
        [Display(Name = "Дата доставки")]
        [Required(ErrorMessage = "Заполните поле.")]
        public string DeliveryDate { get; set; }
        [Display(Name = "Время доставки")]
        [Required(ErrorMessage = "Заполните поле.")]
        public string DeliveryTime { get; set; }

        public List<BasketFlower> Flowers { get; set; } = new List<BasketFlower>();
        public double GetPrice() => Flowers.Where(f => f.Flower.IsInStock && f.Flower.Status == TypeStatus.Опубликовано).Sum(f => f.GetPrice());
        public double GetTotalPrice() => Flowers.Where(f => f.Flower.IsInStock && f.Flower.Status == TypeStatus.Опубликовано).Sum(f => f.GetTotalPrice());

    }
}
