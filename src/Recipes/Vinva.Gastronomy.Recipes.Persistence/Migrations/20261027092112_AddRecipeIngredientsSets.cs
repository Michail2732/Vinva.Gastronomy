using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vinva.Gastronomy.Recipes.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeIngredientsSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "RecipeIngredients",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<Guid>(
                name: "IngredientsSetId",
                table: "RecipeIngredients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RecipeIngredientsSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PortionsQuantitiesJson = table.Column<string>(type: "jsonb", nullable: false),
                    PortionsCalculateType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredientsSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredientsSets_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Существующие ингредиенты переносятся в наборы по бывшему полю Part ("Основное" для ингредиентов без Part).
            migrationBuilder.Sql(@"
                INSERT INTO ""RecipeIngredientsSets"" (""Id"", ""RecipeId"", ""Name"", ""PortionsQuantitiesJson"", ""PortionsCalculateType"")
                SELECT gen_random_uuid(), src.""RecipeId"", src.""SetName"", '[]'::jsonb, 0
                FROM (
                    SELECT DISTINCT ""RecipeId"", COALESCE(""Part"", 'Основное') AS ""SetName""
                    FROM ""RecipeIngredients""
                ) AS src;

                UPDATE ""RecipeIngredients"" AS i
                SET ""IngredientsSetId"" = s.""Id""
                FROM ""RecipeIngredientsSets"" AS s
                WHERE s.""RecipeId"" = i.""RecipeId"" AND s.""Name"" = COALESCE(i.""Part"", 'Основное');

                ALTER TABLE ""RecipeIngredients"" ALTER COLUMN ""Id"" DROP DEFAULT;
            ");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "Part",
                table: "RecipeIngredients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientsSetId_Name",
                table: "RecipeIngredients",
                columns: new[] { "IngredientsSetId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredientsSets_RecipeId_Name",
                table: "RecipeIngredientsSets",
                columns: new[] { "RecipeId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_RecipeIngredientsSets_IngredientsSetId",
                table: "RecipeIngredients",
                column: "IngredientsSetId",
                principalTable: "RecipeIngredientsSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Part",
                table: "RecipeIngredients",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE ""RecipeIngredients"" AS i
                SET ""Part"" = s.""Name""
                FROM ""RecipeIngredientsSets"" AS s
                WHERE s.""Id"" = i.""IngredientsSetId"" AND s.""Name"" <> 'Основное';
            ");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_RecipeIngredientsSets_IngredientsSetId",
                table: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "RecipeIngredientsSets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_IngredientsSetId_Name",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "IngredientsSetId",
                table: "RecipeIngredients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients",
                columns: new[] { "RecipeId", "Name" });
        }
    }
}
