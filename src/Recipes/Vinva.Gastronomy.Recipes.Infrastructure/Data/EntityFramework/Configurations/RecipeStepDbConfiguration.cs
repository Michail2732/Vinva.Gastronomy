using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Recipes.Domain.Entities;


namespace Vinva.Gastronomy.Recipes.Infrastructure.Data.EntityFramework.Configurations
{
    public class RecipeStepDbConfiguration : DescriptiveEntityDbConfiguration<RecipeStep>
    {        
        protected override void ConfigureProtected(EntityTypeBuilder<RecipeStep> builder)
        {            
            builder.ToTable("RecipeSteps");            

            builder.HasKey(a => new { a.RecipeId, a.SeqNumber });

            builder.Property(a => a.SeqNumber)
                   .IsRequired();

            builder.HasOne<Recipe>()
                   .WithMany()
                   .HasForeignKey(a => a.RecipeId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
