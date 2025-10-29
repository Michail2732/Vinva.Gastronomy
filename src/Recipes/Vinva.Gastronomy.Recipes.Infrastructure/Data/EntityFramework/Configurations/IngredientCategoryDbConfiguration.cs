using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Infrastructure.Data.EntityFramework.Configurations
{
    public class IngredientCategoryDbConfiguration : DescriptiveEntityDbConfiguration<IngredientCategory>
    {        
        protected override void ConfigureProtected(EntityTypeBuilder<IngredientCategory> builder)
        {
            builder.ToTable("IngredientCategories");

            builder.HasKey(a => new { a.IngredientId, a.Name });

            builder.HasOne<Ingredient>()
                   .WithMany()
                   .HasForeignKey(a => a.IngredientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.Name);                   
        }
    }
}
