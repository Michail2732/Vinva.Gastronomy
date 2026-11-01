using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Persistence.Models;

namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public class CharacteristicDefinitionDbConfiguration : IEntityTypeConfiguration<CharacteristicDefinitionRecord>
    {
        public void Configure(EntityTypeBuilder<CharacteristicDefinitionRecord> builder)
        {            
            builder.ToTable("CharacteristicDefinitions");

            builder.HasKey(a => new { a.Name, a.Type});

            builder.Property(a => a.Name)
                   .HasMaxLength(CommonConstants.MaxLengthName)
                   .IsRequired();

            builder.Property(a => a.Measure)
                   .HasMaxLength(CommonConstants.MaxLengthMeasure);

            builder.Property(a => a.Type)
                   .IsRequired();
        }
    }
}
