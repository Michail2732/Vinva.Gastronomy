using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence.Converters;

namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public class RecipeIngredientDbConfiguration : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {            
            builder.ToTable("RecipeIngredients");

            builder.HasKey(a => new { a.RecipeId, a.IngredientId });

            builder.Property(a => a.Name)
                   .HasMaxLength(CommonConstants.MaxLengthName)
                   .IsRequired();            

            builder.Property(a => a.Comment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);

            builder.HasOne<Recipe>()
                   .WithMany(a => a.Ingredients)
                   .HasForeignKey(a => a.RecipeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Ingredient>()
                   .WithMany()
                   .HasForeignKey(a => a.IngredientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.Quantities)
                   .HasConversion<IngredientQuantitiesConverter>()
                   .HasMaxLength(128)
                   .IsRequired(true)
                   .HasComment("Format: quantity1:unit1;quantity2:unit2;...");

            builder.Property(a => a.IsRequired).IsRequired();
        }
    }
}
