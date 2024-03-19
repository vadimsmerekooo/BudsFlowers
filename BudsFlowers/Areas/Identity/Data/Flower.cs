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
        public bool IsInStock { get; set; }
        public bool IsPopular { get; set; }
        public TypeStatus Status { get; set; }
        public bool IsSale() => this.Sale > 0 && this.Sale <= 100;

        public virtual FlowerCategory Category { get; set; }
        public virtual List<Review> Reviews { get; set; } = new List<Review>();
    }
}
