using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudsFlowers.Migrations
{
    /// <inheritdoc />
    public partial class changemodelcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowerCategories_FlowerCategories_ParentFlowerCategoryId",
                table: "FlowerCategories");

            migrationBuilder.DropIndex(
                name: "IX_FlowerCategories_ParentFlowerCategoryId",
                table: "FlowerCategories");

            migrationBuilder.DropColumn(
                name: "TypeCategory",
                table: "Flowers");

            migrationBuilder.DropColumn(
                name: "ParentFlowerCategoryId",
                table: "FlowerCategories");

            migrationBuilder.AddColumn<int>(
                name: "TypeCategory",
                table: "FlowerCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TypeStatus",
                table: "FlowerCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeCategory",
                table: "FlowerCategories");

            migrationBuilder.DropColumn(
                name: "TypeStatus",
                table: "FlowerCategories");

            migrationBuilder.AddColumn<int>(
                name: "TypeCategory",
                table: "Flowers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ParentFlowerCategoryId",
                table: "FlowerCategories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlowerCategories_ParentFlowerCategoryId",
                table: "FlowerCategories",
                column: "ParentFlowerCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowerCategories_FlowerCategories_ParentFlowerCategoryId",
                table: "FlowerCategories",
                column: "ParentFlowerCategoryId",
                principalTable: "FlowerCategories",
                principalColumn: "Id");
        }
    }
}
