using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public class IngredientDbConfiguration : IEntityTypeConfiguration<Ingredient>
    {        
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {            
            builder.ToTable("Ingredients");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                   .HasMaxLength(CommonConstants.MaxLengthName)
                   .IsRequired();

            builder.Property(a => a.Description)
                   .HasMaxLength(CommonConstants.MaxLengthDescription)
                   .IsRequired();

            builder.HasIndex(a => a.Name)
                   .IsUnique();

            builder.HasOne<Recipe>()
                   .WithMany()
                   .HasForeignKey(a => a.RecipeId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
