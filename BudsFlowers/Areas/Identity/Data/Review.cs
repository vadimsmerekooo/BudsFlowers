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
        [Required(ErrorMessage = "Заполните поле.")]
        public string FirstName { get; set; }
        [Display(Name = "Почта")]
        [Required(ErrorMessage = "Заполните поле.")]
        public string Email { get; set; }
        [Display(Name = "Отзыв")]
        [Required(ErrorMessage = "Заполните поле.")]
        public string Description { get; set; }
        public int Star { get; set; }
        public TypeStatus Status { get; set; }
        public TypeReview Type { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public virtual Flower? Flower { get; set; }
        public virtual User? User { get; set; }
    }
}
