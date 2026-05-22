using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;


namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public class RecipeStepDbConfiguration : IEntityTypeConfiguration<RecipeStep>
    {        
        public void Configure(EntityTypeBuilder<RecipeStep> builder)
        {            
            builder.ToTable("RecipeSteps");            

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Description)
                   .HasMaxLength(CommonConstants.MaxLengthDescription)
                   .IsRequired();

            builder.Property(a => a.Comment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);

            builder.Property(a => a.SeqNumber)
                   .IsRequired();

            builder.HasOne<Recipe>()
                   .WithMany(a => a.Steps)
                   .HasForeignKey(a => a.RecipeId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
