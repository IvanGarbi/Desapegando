using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desapegando.Data.Migrations
{
    public partial class PropriedadeAtivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Condomino",
                type: "BIT",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Condomino");
        }
    }
}
