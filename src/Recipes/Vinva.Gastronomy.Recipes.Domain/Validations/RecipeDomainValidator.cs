using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Domain.Validations
{
    public class RecipeDomainValidator
    {
        public static bool ValidateIngredientMeasure(string measure)
        {
            return Regex.IsMatch(measure, RegexFormats.IngredientMeasure);
        }

        public static bool ValidateIngredientQuantity(decimal quantity)
        {
            return quantity > 0;
        }

        public static bool ValidateName(string name)
        {
            return Regex.IsMatch(name, RegexFormats.Name);
        }

        public static bool ValidateDescription(string description)
        {
            return Regex.IsMatch(description, RegexFormats.Description);
        }

        public static bool ValidateComment(string comment)
        {
            return Regex.IsMatch(comment, RegexFormats.Comment);
        }

        public static bool ValidateCookingTime(TimeSpan time)
        {
            return TimeSpan.FromMinutes(1) <= time &&
                TimeSpan.FromHours(72) >= time;
        }        

    }
}
