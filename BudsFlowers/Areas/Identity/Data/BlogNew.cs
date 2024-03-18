using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudsFlowers.Areas.Identity.Data
{
    public class BlogNew
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string PreviewPhotoPath { get; set; }
        public string PreviewDescription { get; set; }
        public string Description { get; set; }
        public TypeStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
