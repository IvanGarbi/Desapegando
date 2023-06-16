using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desapegando.Data.Migrations
{
    public partial class NovosCamposCondominoProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataVenda",
                table: "Produto",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataRegistro",
                table: "Condomino",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataVenda",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "DataRegistro",
                table: "Condomino");
        }
    }
}
