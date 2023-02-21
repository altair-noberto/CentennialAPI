using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentennialAPI.Classes.Usuarios
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30)]
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public byte[]? PasswordHash { get; set; }
        [Required]
        public byte[]? PasswordSalt { get; set; }
        [Required]
        [StringLength(30)]
        public string Role { get; set; } = string.Empty;
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;
        [StringLength(80)]
        public string Email { get; set; } = string.Empty;
        public List<Comment>? Comments { get; set; }
    }
}