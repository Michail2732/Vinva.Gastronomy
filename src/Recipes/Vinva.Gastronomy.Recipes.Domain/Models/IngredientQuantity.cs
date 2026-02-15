using System.Text.RegularExpressions;
using Vinva.Gastronomy.Recipes.Domain.Exceptions;
using Vinva.Gastronomy.Recipes.Domain.Validations;

namespace Vinva.Gastronomy.Recipes.Domain.Models
{
    public readonly record struct IngredientQuantity
    {
        public required decimal Quantity { get; init; }
        public required string Measure { get; init; }

        public override string ToString()
        {
            return $"{Quantity}:{Measure}";
        }

        public static IngredientQuantity Parse(string str)
        {
            var splitedStr = str.Trim().Trim(';').Split(':');
            if (splitedStr.Length == 2 || !decimal.TryParse(splitedStr[0], out var quantity))
                throw new RecipeDomainException(typeof(IngredientQuantity), RecipeDomainErrors.FailedParseIngredientQuantity(str));
            
            return new IngredientQuantity
            {
                Quantity = quantity,
                Measure = splitedStr[1]
            };
        }        
    }

}
