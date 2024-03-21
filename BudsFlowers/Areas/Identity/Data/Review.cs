using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public int Star { get; set; }
        public TypeStatus Status { get; set; }
        public DateTime Date { get; set; }
        public virtual Flower? Flower { get; set; }
        public virtual User? User { get; set; }
    }
}
