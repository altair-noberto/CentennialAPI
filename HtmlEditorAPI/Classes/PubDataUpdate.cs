using System.ComponentModel.DataAnnotations.Schema;

namespace HtmlEditorAPI.Classes
{
    [NotMapped]
    public class PubDataUpdate
    {
        public string id { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public IFormFile img { get; set; }
        public IFormFile pub { get; set; }
    }
}
