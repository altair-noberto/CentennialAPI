using Microsoft.EntityFrameworkCore;
using HtmlEditorAPI.Classes;

namespace HtmlEditorAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Publicacao> Publicacoes { get; set; }
    }
}