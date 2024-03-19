using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class Order:Basket
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public double TotalPrice { get; set; }
        public double TotalSale { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateClose { get; set; }

        public virtual List<OrderItem> Flowers { get; set; }
        public virtual User? User { get; set; }
    }
}
