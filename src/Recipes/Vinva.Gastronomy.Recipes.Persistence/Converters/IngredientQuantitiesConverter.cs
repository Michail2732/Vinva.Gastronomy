using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Recipes.Domain.Models;

namespace Vinva.Gastronomy.Recipes.Persistence.Converters
{
    public class IngredientQuantitiesConverter : ValueConverter<IngredientQuantities, string>
    {
        public IngredientQuantitiesConverter() : base(a => a.ToString(), a => IngredientQuantities.Parse(a))
        {

        }
    }
}
