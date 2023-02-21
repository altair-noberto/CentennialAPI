using System.ComponentModel.DataAnnotations.Schema;

namespace CentennialAPI.Classes
{
    [NotMapped]
    public class CommentModel
    {
        public string CommentText { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int PubId { get; set; }
    }
}
