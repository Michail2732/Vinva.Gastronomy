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
    public class RecipeDbConfiguration : DescriptiveEntityDbConfiguration<Recipe>
    {
        public override void Configure(EntityTypeBuilder<Recipe> builder)
        {
            base.Configure(builder);
            builder.ToTable("Recipes");

            builder.HasKey(x => x.Id);

            builder.HasIndex(a => a.Name)
                   .IsUnique();            

            builder.Property(a => a.CookingComment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);

            builder.Property(a => a.IngredientComment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);

            builder.Property(a => a.StorageComment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);

            builder.Property(a => a.UsageComment)
                   .HasMaxLength(CommonConstants.MaxLengthComment);

            builder.Property(a => a.OtherImageIds)
                   .HasColumnType("uuid[]")
                   .HasDefaultValueSql("'{}'::uuid[]");

            builder.HasOne<Recipe>()
                   .WithMany()                   
                   .HasForeignKey(a => a.BaseRecipe)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(a => a.Steps)
                   .WithOne()
                   .HasForeignKey(a => a.RecipeId);

            builder.HasMany(a => a.Ingredients)
                   .WithOne()
                   .HasForeignKey(a => a.RecipeId);            

            builder.HasQueryFilter(b => !b.IsDeleted);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };

            var converter = new ValueConverter<RecipeProperties, string>(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<RecipeProperties>(v, jsonOptions)!
            );

            var comparer = new ValueComparer<RecipeProperties>(
                (a, b) => JsonSerializer.Serialize(a, jsonOptions) == JsonSerializer.Serialize(b, jsonOptions),
                v => JsonSerializer.Serialize(v, jsonOptions).GetHashCode(),
                v => JsonSerializer.Deserialize<RecipeProperties>(
                        JsonSerializer.Serialize(v, jsonOptions),
                        jsonOptions)!
            );

            builder.Property(x => x.Properties)
                   .HasConversion(converter)
                   .Metadata
                   .SetValueComparer(comparer);

                           builder.Property(x => x.Properties)
                               .HasColumnType("jsonb");
                       }
                   }
}
