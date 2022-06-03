using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeiraWebAPI.DAL.Migrations
{
    public partial class Migration00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albuns",
                columns: table => new
                {
                    IdAlbum = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Artista = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    AnoLancamento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albuns", x => x.IdAlbum);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Albuns");
        }
    }
}
