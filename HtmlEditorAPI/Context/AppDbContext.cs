using Microsoft.EntityFrameworkCore;
using HtmlEditorAPI.Classes;
using CentennialAPI.Classes.Usuarios;
using CentennialAPI.Classes;

namespace HtmlEditorAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Publicacao> Publicacoes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}