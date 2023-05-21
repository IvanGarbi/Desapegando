using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desapegando.Data.Migrations
{
    public partial class ProdutoCRUD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Administrador",
                table: "Condomino");

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR(250)", maxLength: 250, nullable: false),
                    Categoria = table.Column<int>(type: "int", nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Preco = table.Column<decimal>(type: "DECIMAL", nullable: false),
                    Ativo = table.Column<bool>(type: "BIT", nullable: false),
                    Desistencia = table.Column<bool>(type: "BIT", nullable: false),
                    EstadoProduto = table.Column<int>(type: "int", nullable: false),
                    Curtida = table.Column<int>(type: "INT", nullable: false),
                    Quantidade = table.Column<int>(type: "INT", nullable: false),
                    CondominoId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_Condomino_CondominoId",
                        column: x => x.CondominoId,
                        principalTable: "Condomino",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoImagem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    ProdutoId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoImagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoImagem_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_CondominoId",
                table: "Produto",
                column: "CondominoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoImagem_ProdutoId",
                table: "ProdutoImagem",
                column: "ProdutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoImagem");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.AddColumn<bool>(
                name: "Administrador",
                table: "Condomino",
                type: "BIT",
                nullable: false,
                defaultValue: false);
        }
    }
}
