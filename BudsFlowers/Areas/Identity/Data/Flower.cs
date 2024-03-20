using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class Flower
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public long Article { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Sale { get; set; } = 0;
        public string Compound { get; set; }
        public string PhotoPath { get; set; }
        [Display(Name = "В наличии")]
        public bool IsInStock { get; set; }
        [Display(Name = "Популярное")]
        public bool IsPopular { get; set; }
        public TypeStatus Status { get; set; }
        public bool IsSale() => this.Sale > 0 && this.Sale <= 100;

        public virtual FlowerCategory Category { get; set; }
        public virtual List<Review> Reviews { get; set; } = new List<Review>();
        [NotMapped]
        public virtual List<SelectListItem> Categories { get; set; }
        [NotMapped]
        public int Orders { get; set; }
        public int GetStar()
        {
            if (Reviews.Count == 0)
                return 0;
            double star = Reviews.Sum(s => s.Star) / Reviews.Count;
            return Convert.ToInt32(Math.Round(star));
        }
    }
}
