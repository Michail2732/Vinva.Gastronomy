using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public class CharacteristicDefinitionDbConfiguration : IEntityTypeConfiguration<CharacteristicDefinition>
    {
        public void Configure(EntityTypeBuilder<CharacteristicDefinition> builder)
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
