using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desapegando.Data.Migrations
{
    public partial class ComprasTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataVenda",
                table: "Produto");

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    CondominoId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    DataVenda = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compras_Condomino_CondominoId",
                        column: x => x.CondominoId,
                        principalTable: "Condomino",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Compras_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compras_CondominoId",
                table: "Compras",
                column: "CondominoId");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_ProdutoId",
                table: "Compras",
                column: "ProdutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataVenda",
                table: "Produto",
                type: "DATETIME",
                nullable: true);
        }
    }
}
