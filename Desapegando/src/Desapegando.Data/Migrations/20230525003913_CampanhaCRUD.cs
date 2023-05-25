using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desapegando.Data.Migrations
{
    public partial class CampanhaCRUD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campanha",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    NomeInstituicao = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    NomeResponsavel = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    EmailResponsavel = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    TelefoneResponsavel = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    LocalDeEncontro = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "DATE", nullable: false),
                    DataFinal = table.Column<DateTime>(type: "DATE", nullable: false),
                    Ativo = table.Column<bool>(type: "BIT", nullable: false),
                    CondominoId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campanha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campanha_Condomino_CondominoId",
                        column: x => x.CondominoId,
                        principalTable: "Condomino",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampanhaImagem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false),
                    CampanhaId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampanhaImagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampanhaImagem_Campanha_CampanhaId",
                        column: x => x.CampanhaId,
                        principalTable: "Campanha",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campanha_CondominoId",
                table: "Campanha",
                column: "CondominoId");

            migrationBuilder.CreateIndex(
                name: "IX_CampanhaImagem_CampanhaId",
                table: "CampanhaImagem",
                column: "CampanhaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampanhaImagem");

            migrationBuilder.DropTable(
                name: "Campanha");
        }
    }
}
