using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Persistence.Configurations
{
    public class RecipeDbConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {            
            builder.ToTable("Recipes");

            builder.HasKey(x => x.Id);

            builder.Property(a => a.Name)
                   .HasMaxLength(CommonConstants.MaxLengthName)
                   .IsRequired();

            builder.Property(a => a.Description)
                   .HasMaxLength(CommonConstants.MaxLengthMeasure)
                   .IsRequired();

            builder.HasIndex(a => a.Name)
                   .IsUnique();                        

            builder.Property(a => a.GalleryImageIds)
                   .HasColumnType("uuid[]")
                   .HasDefaultValueSql("'{}'::uuid[]");

            builder.HasOne<Recipe>()
                   .WithMany()                   
                   .HasForeignKey(a => a.BaseRecipe)
                   .OnDelete(DeleteBehavior.SetNull);            

            builder.Ignore(a => a.Characteristics);
              

            
        }
    }
}
