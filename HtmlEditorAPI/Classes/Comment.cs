using CentennialAPI.Classes.Usuarios;
using HtmlEditorAPI.Classes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentennialAPI.Classes
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [StringLength(500)]
        public string CommentText { get; set; } = string.Empty;
        [ForeignKey("Users")]
        public int UserId { get; set; }
        [ForeignKey("Publicacao")]
        public int PublicacaoId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
