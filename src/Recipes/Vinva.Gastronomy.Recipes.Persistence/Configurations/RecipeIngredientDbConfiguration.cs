using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Persistence.Models;

namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public sealed class RecipeIngredientDbConfiguration : IEntityTypeConfiguration<RecipeIngredientRecord>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredientRecord> builder)
        {
            builder.ToTable("RecipeIngredients");
            builder.HasKey(item => item.Id);
            builder.Property(item => item.Id).ValueGeneratedNever();
            builder.HasIndex(item => new { item.IngredientsSetId, item.Name })
                .IsUnique();
            builder.HasOne<RecipeRecord>()
                .WithMany()
                .HasForeignKey(item => item.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<RecipeIngredientsSetRecord>()
                .WithMany(set => set.Ingredients)
                .HasForeignKey(item => item.IngredientsSetId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(item => item.Name)
                .HasMaxLength(CommonConstants.MaxLengthName)
                .IsRequired();
            builder.Property(item => item.QuantitiesJson)
                .HasColumnType("jsonb")
                .IsRequired();
        }
    }
}
