using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vinva.Gastronomy.Recipes.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_base_recipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Recipes_BaseRecipe",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_BaseRecipe",
                table: "Recipes");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_BaseRecipe",
                table: "Recipes",
                column: "BaseRecipe");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Recipes_BaseRecipe",
                table: "Recipes",
                column: "BaseRecipe",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Recipes_BaseRecipe",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_BaseRecipe",
                table: "Recipes");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_BaseRecipe",
                table: "Recipes",
                column: "BaseRecipe",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Recipes_BaseRecipe",
                table: "Recipes",
                column: "BaseRecipe",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
