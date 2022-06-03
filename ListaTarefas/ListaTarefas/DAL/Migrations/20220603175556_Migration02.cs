using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ListaTarefas.DAL.Migrations
{
    public partial class Migration02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Responsaveis",
                columns: table => new
                {
                    IdResponsavel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTarefa = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsaveis", x => x.IdResponsavel);
                    table.ForeignKey(
                        name: "FK_Responsaveis_Tarefas_IdTarefa",
                        column: x => x.IdTarefa,
                        principalTable: "Tarefas",
                        principalColumn: "IdTarefa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Responsaveis_IdTarefa",
                table: "Responsaveis",
                column: "IdTarefa");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Responsaveis");
        }
    }
}
