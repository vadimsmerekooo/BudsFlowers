using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudsFlowers.Migrations
{
    /// <inheritdoc />
    public partial class Upmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeCategory",
                table: "Flowers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeCategory",
                table: "Flowers");
        }
    }
}
