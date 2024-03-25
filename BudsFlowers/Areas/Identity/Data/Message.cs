using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Введите Имя.")]
        [StringLength(50, ErrorMessage = "Длина {0} должна быть не менее {2} и не более {1} символов.", MinimumLength = 3)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Phone]
        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Введите телефон.")]
        public string Phone { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Введите почту.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Введите сообщение.")]
        [StringLength(50, ErrorMessage = "Длина {0} должна быть не менее {2} и не более {1} символов.", MinimumLength = 3)]
        [Display(Name = "Текст сообщения")]
        public string Text { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public virtual User? User { get; set; }
    }
}
