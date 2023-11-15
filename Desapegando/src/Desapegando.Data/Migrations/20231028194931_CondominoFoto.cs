using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desapegando.Data.Migrations
{
    public partial class CondominoFoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Condomino",
                type: "NVARCHAR(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Condomino");
        }
    }
}
