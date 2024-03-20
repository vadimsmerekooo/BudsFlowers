using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class FlowerCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "Длина {0} должна быть не менее {2} и не более {1} символов.", MinimumLength = 3)]
        public string Title { get; set; }
        public string PreviewPhotoPath { get; set; }
        public TypeCategory TypeCategory { get; set; }
        public TypeStatus TypeStatus { get; set; }

        public virtual List<Flower> Flowers { get; set; } = new List<Flower>();

    }
}
