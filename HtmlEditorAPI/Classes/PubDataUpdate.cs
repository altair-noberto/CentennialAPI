namespace HtmlEditorAPI.Classes
{
    public class PubDataUpdate
    {
        public string id { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public IFormFile img { get; set; }
        public IFormFile pub { get; set; }
    }
}
