using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vinva.Gastronomy.Recipes.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMeasureToQuantities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Measure",
                table: "RecipeIngredients");

            migrationBuilder.AddColumn<string>(
                name: "Quantities",
                table: "RecipeIngredients",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                comment: "Format: quantity1:unit1;quantity2:unit2;...");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantities",
                table: "RecipeIngredients");

            migrationBuilder.AddColumn<string>(
                name: "Measure",
                table: "RecipeIngredients",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
