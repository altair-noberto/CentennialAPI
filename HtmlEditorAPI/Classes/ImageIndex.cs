namespace HtmlEditorAPI.Classes
{
    public class ImageIndex
    {
        public ImageIndex(string nome, string url)
        {
            Nome = nome;
            Url = url;
        }
        public string Nome { get; set; }
        public string Url { get; set; }
    }
}
