using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public class IngredientDbConfiguration : DescriptiveEntityDbConfiguration<Ingredient>
    {        
        public override void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            base.Configure(builder);
            builder.ToTable("Ingredients");

            builder.HasKey(a => a.Id);

            builder.HasIndex(a => a.Name)
                   .IsUnique();

            builder.HasOne<Recipe>()
                   .WithMany()
                   .HasForeignKey(a => a.RecipeId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.Comment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);            

            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
