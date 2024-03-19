using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public double Sale { get; set; }
        public virtual Order Order { get; set; }
        public virtual Flower Flower { get; set; }
    }
}
