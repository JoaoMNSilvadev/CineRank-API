using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineRank.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueAvaliacaoUsuarioFilme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_UsuarioId",
                table: "Avaliacoes");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_UsuarioId_FilmeId",
                table: "Avaliacoes",
                columns: new[] { "UsuarioId", "FilmeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_UsuarioId_FilmeId",
                table: "Avaliacoes");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_UsuarioId",
                table: "Avaliacoes",
                column: "UsuarioId");
        }
    }
}
