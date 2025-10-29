using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Infrastructure.Data.EntityFramework.Configurations
{
    public class RecipeCategoryDbConfiguration : DescriptiveEntityDbConfiguration<RecipeCategory>
    {
        protected override void ConfigureProtected(EntityTypeBuilder<RecipeCategory> builder)
        {
            builder.ToTable("RecipeCategories");

            builder.HasKey(a => new {a.RecipeId, a.Name});

            builder.HasOne<Recipe>()
                   .WithMany()
                   .HasForeignKey(a => a.RecipeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
