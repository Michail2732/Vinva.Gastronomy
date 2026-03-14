using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Validations;

namespace Vinva.Gastronomy.Recipes.Domain.Validations
{
    public class RecipeDomainValidator : DescriptiveEntityValidator
    {
        public const string IngredientMeasure = DescriptiveEntityValidationRegex.AlphanumericWithSpacesDotBrace;

        public static bool ValidateIngredientMeasure(string measure)
        {
            return ValidateCustom(measure, IngredientMeasure);
        }

        public static bool ValidateIngredientQuantity(decimal quantity)
        {
            return quantity > 0;
        }        

        public static bool ValidateCookingTime(TimeSpan time)
        {
            return TimeSpan.FromMinutes(1) <= time &&
                TimeSpan.FromHours(72) >= time;
        }        

    }
}
