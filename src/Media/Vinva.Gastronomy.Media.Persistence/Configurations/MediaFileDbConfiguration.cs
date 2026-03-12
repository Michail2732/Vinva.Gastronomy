using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Media.Domain.Entities;

namespace Vinva.Gastronomy.Media.Persistence.Configurations
{
    public class MediaFileDbConfiguration : IEntityTypeConfiguration<MediaItem>
    {
        public void Configure(EntityTypeBuilder<MediaItem> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Path)
                .HasMaxLength(CommonConstants.MaxLengthPath)
                .IsRequired();

            builder.Property(a => a.Name)
                .HasMaxLength(CommonConstants.MaxLengthName)
                .IsRequired();            
        }
    }
}
