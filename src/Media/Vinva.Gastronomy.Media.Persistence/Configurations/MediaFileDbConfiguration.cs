using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Media.Domain.Entities;
using Vinva.Gastronomy.Media.Persistence.Converters;

namespace Vinva.Gastronomy.Media.Persistence.Configurations
{
    public class MediaFileDbConfiguration : IEntityTypeConfiguration<ImageMetadata>
    {
        public void Configure(EntityTypeBuilder<ImageMetadata> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Format)                
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(a => a.Name)
                .HasMaxLength(CommonConstants.MaxLengthName)
                .IsRequired();            
        }
    }
}
