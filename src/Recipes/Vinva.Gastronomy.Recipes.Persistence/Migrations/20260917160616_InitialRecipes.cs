using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vinva.Gastronomy.Recipes.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacteristicDefinitions",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Measure = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacteristicDefinitions", x => new { x.Name, x.Type });
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    BaseRecipe = table.Column<Guid>(type: "uuid", nullable: true),
                    Document = table.Column<string>(type: "text", nullable: false),
                    TitleImageId = table.Column<Guid>(type: "uuid", nullable: true),
                    GalleryImageIds = table.Column<Guid[]>(type: "uuid[]", nullable: true, defaultValueSql: "'{}'::uuid[]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Recipes_BaseRecipe",
                        column: x => x.BaseRecipe,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Characteristics",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Measure = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    StringValue = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    NumberValue = table.Column<double>(type: "double precision", nullable: true),
                    RangeMin = table.Column<double>(type: "double precision", nullable: true),
                    RangeMax = table.Column<double>(type: "double precision", nullable: true),
                    ArrayItems = table.Column<string[]>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristics", x => new { x.RecipeId, x.Name });
                    table.CheckConstraint("CK_Characteristics_ValueByType", "(\"Type\" = 0 AND \"StringValue\" IS NOT NULL AND \"NumberValue\" IS NULL AND \"RangeMin\" IS NULL AND \"RangeMax\" IS NULL AND \"ArrayItems\" IS NULL) OR (\"Type\" = 1 AND \"StringValue\" IS NULL AND \"NumberValue\" IS NOT NULL AND \"RangeMin\" IS NULL AND \"RangeMax\" IS NULL AND \"ArrayItems\" IS NULL) OR (\"Type\" = 2 AND \"StringValue\" IS NULL AND \"NumberValue\" IS NULL AND \"RangeMin\" IS NOT NULL AND \"RangeMax\" IS NOT NULL AND \"ArrayItems\" IS NULL) OR (\"Type\" = 3 AND \"StringValue\" IS NULL AND \"NumberValue\" IS NULL AND \"RangeMin\" IS NULL AND \"RangeMax\" IS NULL AND \"ArrayItems\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Characteristics_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_BaseRecipe",
                table: "Recipes",
                column: "BaseRecipe");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_Name",
                table: "Recipes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacteristicDefinitions");

            migrationBuilder.DropTable(
                name: "Characteristics");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
