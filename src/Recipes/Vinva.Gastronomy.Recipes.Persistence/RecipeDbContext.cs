using Microsoft.EntityFrameworkCore;
using System;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence.Configurations;
using Vinva.Gastronomy.Recipes.Persistence.Models;

namespace Vinva.Gastronomy.Recipes.Persistence
{
    public class RecipeDbContext: DbContext
    {
        public DbSet<Recipe> Recipes { get; private init; }
        public DbSet<CharacteristicRecord> CharacteristicRecords { get; private init; }
        public DbSet<RecipeIngredientRecord> RecipeIngredientRecords { get; private init; }
        public DbSet<CharacteristicDefinition> CharacteristicDefinitions { get; private init; }

        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.ApplyConfiguration(new RecipeDbConfiguration());
            modelBuilder.ApplyConfiguration(new CharacteristicDbConfiguration());            
            modelBuilder.ApplyConfiguration(new RecipeIngredientDbConfiguration());
            modelBuilder.ApplyConfiguration(new CharacteristicDefinitionDbConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
