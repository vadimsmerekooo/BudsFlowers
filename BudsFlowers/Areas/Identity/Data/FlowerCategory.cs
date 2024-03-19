using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class FlowerCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string PreviewPhotoPath { get; set; }

        public virtual FlowerCategory? ParentFlowerCategory { get; set; }
        public virtual List<FlowerCategory> ChildFlowerCategories { get; set; } = new List<FlowerCategory>();
        public virtual List<Flower> Flowers { get; set; } = new List<Flower>();

    }
}
