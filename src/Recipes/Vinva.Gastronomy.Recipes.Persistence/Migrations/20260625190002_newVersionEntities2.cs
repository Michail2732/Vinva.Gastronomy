using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vinva.Gastronomy.Recipes.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class newVersionEntities2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<Guid>>(
                name: "OtherImageIds",
                table: "Recipes",
                type: "uuid[]",
                nullable: false,
                defaultValueSql: "'{}'::uuid[]",
                oldClrType: typeof(List<Guid>),
                oldType: "uuid[]",
                oldDefaultValue: new List<Guid>());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<Guid>>(
                name: "OtherImageIds",
                table: "Recipes",
                type: "uuid[]",
                nullable: false,
                defaultValue: new List<Guid>(),
                oldClrType: typeof(List<Guid>),
                oldType: "uuid[]",
                oldDefaultValueSql: "'{}'::uuid[]");
        }
    }
}
