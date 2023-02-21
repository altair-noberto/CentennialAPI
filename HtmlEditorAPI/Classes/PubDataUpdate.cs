using System.ComponentModel.DataAnnotations.Schema;

namespace HtmlEditorAPI.Classes
{
    [NotMapped]
    public class PubDataUpdate
    {
        public int id { get; set; }
        public string title { get; set; } = string.Empty;
        public string subtitle { get; set; } = string.Empty;
        public IFormFile? img { get; set; }
        public IFormFile? pub { get; set; }
    }
}
