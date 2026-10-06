using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vinva.Gastronomy.Recipes.Persistence.Migrations
{
    [DbContext(typeof(RecipeDbContext))]
    [Migration("20261003090000_AddRecipeIngredientPart")]
    public partial class AddRecipeIngredientPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Part",
                table: "RecipeIngredients",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Part",
                table: "RecipeIngredients");
        }
    }
}