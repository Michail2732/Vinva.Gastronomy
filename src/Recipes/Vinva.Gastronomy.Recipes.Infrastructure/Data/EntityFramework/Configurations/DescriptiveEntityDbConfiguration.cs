using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Recipes.Domain.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Infrastructure.Data.EntityFramework.Configurations
{
    public abstract class DescriptiveEntityDbConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : DescriptiveEntity        
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ConfigureProtected(builder);
            builder.Property(a => a.Name)
                   .HasMaxLength(CommonConstants.MaxLengthName)
                   .IsRequired();

            builder.Property(a => a.Description)
                   .HasMaxLength(CommonConstants.MaxLengthDescription)
                   .IsRequired();

            builder.Property(a => a.Comment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);            
        }        

        protected abstract void ConfigureProtected(EntityTypeBuilder<TEntity> builder);
    }
}
