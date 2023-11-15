using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desapegando.Data.Migrations
{
    public partial class DataDesistencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataDesistencia",
                table: "Produto",
                type: "DATETIME",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataDesistencia",
                table: "Produto");
        }
    }
}
