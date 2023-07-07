using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCPeliculas.Migrations
{
    public partial class peliculadeseada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PeliculaDeseada",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(nullable: false),
                    PeliculaId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculaDeseada", x => new { x.UsuarioId, x.PeliculaId });
                    table.ForeignKey(
                        name: "FK_PeliculaDeseada_Pelicula_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Pelicula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculaDeseada_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaDeseada_PeliculaId",
                table: "PeliculaDeseada",
                column: "PeliculaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeliculaDeseada");
        }
    }
}
