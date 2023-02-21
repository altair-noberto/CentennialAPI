using System.ComponentModel.DataAnnotations.Schema;

namespace HtmlEditorAPI.Classes
{
    [NotMapped]
    public class ImageModel
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile? FormFile { get; set; }
    }
}
