using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Validations
{
    public class DescriptiveEntityValidator
    {
        protected static readonly TimeSpan _regexTimeout = TimeSpan.FromSeconds(2);

        public static bool ValidateName(string name)
        {
            return Regex.IsMatch(name, DescriptiveEntityValidationRegex.Name, RegexOptions.None, _regexTimeout);
        }

        public static bool ValidateDescription(string description)
        {
            return Regex.IsMatch(description, DescriptiveEntityValidationRegex.Description, RegexOptions.None, _regexTimeout);
        }

        public static bool ValidateComment(string comment)
        {
            return Regex.IsMatch(comment, DescriptiveEntityValidationRegex.Comment, RegexOptions.None, _regexTimeout);
        }                

        public static bool ValidateCustom(string propertyVal, string regexPattern)
        {
            return Regex.IsMatch(propertyVal, regexPattern, RegexOptions.None, _regexTimeout);
        }
    }
}
