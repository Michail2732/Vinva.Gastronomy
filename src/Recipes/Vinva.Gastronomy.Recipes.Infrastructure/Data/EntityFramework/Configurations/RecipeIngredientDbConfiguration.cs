using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Infrastructure.Data.EntityFramework.Configurations
{
    public class RecipeIngredientDbConfiguration : DescriptiveEntityDbConfiguration<RecipeIngredient>
    {
        protected override void ConfigureProtected(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.ToTable("RecipeIngredients");

            builder.HasKey(a => new { a.RecipeId, a.IngredientId });

            builder.HasOne<Recipe>()
                   .WithMany()
                   .HasForeignKey(a => a.RecipeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Ingredient>()
                   .WithMany()
                   .HasForeignKey(a => a.IngredientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.IsRequired).IsRequired();
        }
    }
}
