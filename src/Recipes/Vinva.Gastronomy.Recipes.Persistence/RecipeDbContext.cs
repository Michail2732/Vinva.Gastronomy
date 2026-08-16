using Microsoft.EntityFrameworkCore;
using System;
using Vinva.Gastronomy.Recipes.Domain.Entities;
using Vinva.Gastronomy.Recipes.Persistence.Configurations;

namespace Vinva.Gastronomy.Recipes.Persistence
{
    public class RecipeDbContext: DbContext
    {
        public DbSet<Recipe> Recipes { get; private init; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; private init; }                
        public DbSet<Ingredient> Ingredients { get; private init; }

        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.ApplyConfiguration(new IngredientDbConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeDbConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeIngredientDbConfiguration());              
            base.OnModelCreating(modelBuilder);
        }
    }
}
