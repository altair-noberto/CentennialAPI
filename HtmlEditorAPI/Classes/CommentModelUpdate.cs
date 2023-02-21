using System.ComponentModel.DataAnnotations.Schema;

namespace CentennialAPI.Classes
{
    [NotMapped]
    public class CommentModelUpdate
    {
        public int id { get; set; }
        public string NewComment { get; set; } = string.Empty;
    }
}
