using Microsoft.EntityFrameworkCore;
using System;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Infrastructure.Data.EntityFramework
{
    public class RecipeDbContext: DbContext
    {
        public DbSet<Recipe> Recipes { get; private init; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; private init; }
        public DbSet<RecipeCategory> RecipeCategories { get; private init; }
        public DbSet<RecipeStep> RecipeSteps { get; private set; }
        public DbSet<Ingredient> Ingredients { get; private init; }
        public DbSet<IngredientCategory> IngredientCategories { get; private init; }

        public RecipeDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
