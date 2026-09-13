using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence.Models;

namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public class CharacteristicDbConfiguration : IEntityTypeConfiguration<CharacteristicRecord>
    {        
        public void Configure(EntityTypeBuilder<CharacteristicRecord> builder)
        {            
            builder.ToTable("Characteristics");

            builder.HasKey(a => new {a.RecipeId, a.Name });

            builder.HasOne<Recipe>()
                   .WithMany()
                   .HasForeignKey(a => a.RecipeId)
                   .IsRequired(true)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.Name)
                   .HasMaxLength(CommonConstants.MaxLengthName)
                   .IsRequired();

            builder.Property(a => a.Measure)
                     .HasMaxLength(CommonConstants.MaxLengthMeasure);            

                 builder.Property(a => a.StringValue)
                     .HasMaxLength(CommonConstants.MaxLengthMeasure);

                 builder.Property(a => a.ArrayItems)
                     .HasColumnType("text[]");

                 builder.ToTable(table => table.HasCheckConstraint(
                  "CK_Characteristics_ValueByType",
                  "(\"Type\" = 0 AND \"StringValue\" IS NOT NULL AND \"NumberValue\" IS NULL AND \"RangeMin\" IS NULL AND \"RangeMax\" IS NULL AND \"ArrayItems\" IS NULL) OR " +
                  "(\"Type\" = 1 AND \"StringValue\" IS NULL AND \"NumberValue\" IS NOT NULL AND \"RangeMin\" IS NULL AND \"RangeMax\" IS NULL AND \"ArrayItems\" IS NULL) OR " +
                  "(\"Type\" = 2 AND \"StringValue\" IS NULL AND \"NumberValue\" IS NULL AND \"RangeMin\" IS NOT NULL AND \"RangeMax\" IS NOT NULL AND \"ArrayItems\" IS NULL) OR " +
                  "(\"Type\" = 3 AND \"StringValue\" IS NULL AND \"NumberValue\" IS NULL AND \"RangeMin\" IS NULL AND \"RangeMax\" IS NULL AND \"ArrayItems\" IS NOT NULL)"));
        }
    }
}
