using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desapegando.Data.Migrations
{
    public partial class ProdutoCurtida : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdutoCurtida",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    CondominoId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoCurtida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoCurtida_Condomino_CondominoId",
                        column: x => x.CondominoId,
                        principalTable: "Condomino",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProdutoCurtida_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoCurtida_CondominoId",
                table: "ProdutoCurtida",
                column: "CondominoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoCurtida_ProdutoId",
                table: "ProdutoCurtida",
                column: "ProdutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoCurtida");
        }
    }
}
