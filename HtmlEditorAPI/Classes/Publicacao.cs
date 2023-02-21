using CentennialAPI.Classes;
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
        public string Title { get; set; } = string.Empty;
        [StringLength(80)]
        public string SubTitle { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        public string Category { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        public string PubDirectory { get; set; } = string.Empty;
        [Required]
        [StringLength(500)]
        public string ImgDirectory { get; set; } = string.Empty;
        [Required]
        public string PubData { get; set; } = string.Empty;
        public List<Comment>? Comments { get; set; }
    }
}
