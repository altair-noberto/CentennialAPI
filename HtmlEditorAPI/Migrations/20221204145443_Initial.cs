using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HtmlEditorAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Publicacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    PubDirectory = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    PubData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImgDirectory = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacoes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Publicacoes");
        }
    }
}
