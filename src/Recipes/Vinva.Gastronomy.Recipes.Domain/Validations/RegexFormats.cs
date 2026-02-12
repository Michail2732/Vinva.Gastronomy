using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Recipes.Domain.Validations
{
    public class RegexFormats
    {
        public const string AlphanumericWithSpaces = @"^[a-zA-Z0-9 ]+$";
        public const string AlphanumericWithSpacesDotBrace = @"^[a-zA-Z0-9\(\)\[\]\. ]+$";
        public const string AlphanumericWithSpacesDotBracePunctuation = @"^[a-zA-Z0-9\(\)\[\]\.\,\;\: ]+$";


        public const string IngredientMeasure = AlphanumericWithSpacesDotBrace;
        public const string Description = AlphanumericWithSpacesDotBracePunctuation;
        public const string Name = AlphanumericWithSpaces;
        public const string Comment = AlphanumericWithSpacesDotBracePunctuation;                      
    }
}
