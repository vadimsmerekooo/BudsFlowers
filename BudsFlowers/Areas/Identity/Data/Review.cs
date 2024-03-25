using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Почта")]
        public string Email { get; set; }
        [Display(Name = "Отзыв")]
        public string Description { get; set; }
        public int Star { get; set; }
        public TypeStatus Status { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public virtual Flower? Flower { get; set; }
        public virtual User? User { get; set; }
    }
}
