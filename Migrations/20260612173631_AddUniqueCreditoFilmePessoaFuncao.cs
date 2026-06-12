using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineRank.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueCreditoFilmePessoaFuncao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Creditos_FilmeId",
                table: "Creditos");

            migrationBuilder.CreateIndex(
                name: "IX_Creditos_FilmeId_PessoaId_FuncaoId",
                table: "Creditos",
                columns: new[] { "FilmeId", "PessoaId", "FuncaoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Creditos_FilmeId_PessoaId_FuncaoId",
                table: "Creditos");

            migrationBuilder.CreateIndex(
                name: "IX_Creditos_FilmeId",
                table: "Creditos",
                column: "FilmeId");
        }
    }
}
