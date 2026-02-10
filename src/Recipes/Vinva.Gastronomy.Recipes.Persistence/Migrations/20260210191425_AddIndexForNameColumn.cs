using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vinva.Gastronomy.Recipes.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexForNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RecipeSteps_Name",
                table: "RecipeSteps",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_Name",
                table: "Recipes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_Name",
                table: "RecipeIngredients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecipeSteps_Name",
                table: "RecipeSteps");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_Name",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_Name",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");
        }
    }
}
