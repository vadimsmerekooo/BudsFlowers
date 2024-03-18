using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class Flower
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public int Sale { get; set; } = 0;
        public string PhotoPath { get; set; }
    }
}
