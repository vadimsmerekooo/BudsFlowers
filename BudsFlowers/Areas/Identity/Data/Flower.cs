using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace BudsFlowers.Areas.Identity.Data
{
    public class Flower
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        [Required]
        [Display(Name = "Артикул")]
        public long Article { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Required]
        [Range(0, 99999.99)]
        [Display(Name = "Цена")]
        public double Price { get; set; }
        [Range(0, 100)]
        [Display(Name = "Скидка %")]
        public double Sale { get; set; } = 0;
        [Required]
        [Display(Name = "Состав")]
        public string Compound { get; set; }
        [Display(Name = "Фото")]
        public string PhotoPath { get; set; }
        [Display(Name = "В наличии")]
        public bool IsInStock { get; set; }
        [Display(Name = "Популярное")]
        public bool IsPopular { get; set; }
        [Display(Name = "Статус")]
        public TypeStatus Status { get; set; }
        public bool IsSale() => this.Sale > 0 && this.Sale <= 100;

        public virtual FlowerCategory Category { get; set; }
        public virtual List<Review> Reviews { get; set; } = new List<Review>();
        [NotMapped]
        public virtual List<SelectListItem> Categories { get; set; }
        [NotMapped]
        public int Orders { get; set; }
        [NotMapped]
        [Display(Name = "Категория")]
        public string CategorySelectId { get; set; }
        public int GetTotalPrice() => Convert.ToInt32( Price - ((Price * Sale) / 100));
        public int GetStar()
        {
            if (Reviews.Count == 0)
                return 0;
            double star = Reviews.Sum(s => s.Star) / Reviews.Count;
            return Convert.ToInt32(Math.Round(star));
        }
    }
}
