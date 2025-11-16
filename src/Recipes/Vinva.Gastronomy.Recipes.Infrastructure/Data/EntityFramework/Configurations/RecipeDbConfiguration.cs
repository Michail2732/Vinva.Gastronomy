using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vinva.Gastronomy.Common.Constants;
using Vinva.Gastronomy.Recipes.Domain.Constants;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Infrastructure.Data.EntityFramework.Configurations
{
    public class RecipeDbConfiguration : DescriptiveEntityDbConfiguration<Recipe>
    {
        public override void Configure(EntityTypeBuilder<Recipe> builder)
        {
            base.Configure(builder);
            builder.ToTable("Recipes");

            builder.HasKey(x => x.Id);

            builder.Property(a => a.CookingComment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);

            builder.Property(a => a.IngredientComment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);

            builder.Property(a => a.StorageComment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);

            builder.Property(a => a.UsageComment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);

            builder.HasOne<Recipe>()
                   .WithOne()                   
                   .OnDelete(DeleteBehavior.NoAction);            

            builder.HasMany(a => a.Steps)
                   .WithOne()
                   .HasForeignKey(a => a.RecipeId);

            builder.HasMany(a => a.Ingredients)
                   .WithOne()
                   .HasForeignKey(a => a.RecipeId);

            builder.HasMany(a => a.Categories)
                   .WithMany()
                   .UsingEntity(a => a.ToTable("RecipeCategories"));
        }
    }
}
