using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desapegando.Data.Migrations
{
    public partial class CondominoDataNascimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Idade",
                table: "Condomino");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Condomino",
                type: "DATE",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Condomino");

            migrationBuilder.AddColumn<int>(
                name: "Idade",
                table: "Condomino",
                type: "INT",
                nullable: false,
                defaultValue: 0);
        }
    }
}
