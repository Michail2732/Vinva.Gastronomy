using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Persistence.Models;

namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public sealed class RecipeIngredientsSetDbConfiguration : IEntityTypeConfiguration<RecipeIngredientsSetRecord>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredientsSetRecord> builder)
        {
            builder.ToTable("RecipeIngredientsSets");
            builder.HasKey(item => item.Id);
            builder.Property(item => item.Id).ValueGeneratedNever();
            builder.HasIndex(item => new { item.RecipeId, item.Name })
                .IsUnique();
            builder.HasOne<RecipeRecord>()
                .WithMany(recipe => recipe.IngredientsSets)
                .HasForeignKey(item => item.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(item => item.Name)
                .HasMaxLength(CommonConstants.MaxLengthName)
                .IsRequired();
            builder.Property(item => item.PortionsQuantitiesJson)
                .HasColumnType("jsonb")
                .IsRequired();
        }
    }
}
