using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HtmlEditorAPI.Classes
{
    public class Publicacao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(80)]
        public string Title { get; set; }
        [StringLength(80)]
        public string SubTitle { get; set; }
        [Required]
        [StringLength(500)]
        public string PubDirectory { get; set; }
        [Required]
        [StringLength(500)]
        public string ImgDirectory { get; set; }
        [Required]
        public DateTime PubData { get; set; }
    }
}
