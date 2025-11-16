using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Vinva.Gastronomy.Recipes.Domain.Entities;

namespace Vinva.Gastronomy.Recipes.Infrastructure.Data.Specifications
{
    public static class SpecificationsExtensions
    {
        public static ISpecification<Recipe> IncludeAllDependencies(this ISpecification<Recipe> spec)
        {            
            spec.Query
                .Include(a => a.Categories)
                .Include(a => a.Steps)
                .Include(a => a.Ingredients);
            return spec;
        }

    }
}
