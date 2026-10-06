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
            builder.HasKey(item => new { item.RecipeId, item.Name });
            builder.HasOne<Vinva.Gastronomy.Recipes.Domain.Entities.Recipe>()
                .WithMany()
                .HasForeignKey(item => item.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(item => item.Name)
                .HasMaxLength(CommonConstants.MaxLengthName)
                .IsRequired();
            builder.Property(item => item.Part)
                .HasMaxLength(CommonConstants.MaxLengthName);
            builder.Property(item => item.QuantitiesJson)
                .HasColumnType("jsonb")
                .IsRequired();
        }
    }
}
