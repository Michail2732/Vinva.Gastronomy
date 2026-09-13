using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Constants;

namespace Vinva.Gastronomy.Common.Services
{
    public class ValidationService
    {
        protected static readonly TimeSpan _regexTimeout = TimeSpan.FromSeconds(2);

        public static bool ValidateName(string name)
        {
            return Regex.IsMatch(name, ValidationRegexes.Name, RegexOptions.None, _regexTimeout);
        }

        public static bool ValidateDescription(string description)
        {
            return Regex.IsMatch(description, ValidationRegexes.Description, RegexOptions.None, _regexTimeout);
        }

        public static bool ValidateComment(string comment)
        {
            return Regex.IsMatch(comment, ValidationRegexes.Comment, RegexOptions.None, _regexTimeout);
        }                

        public static bool ValidateCustom(string propertyVal, string regexPattern)
        {
            return Regex.IsMatch(propertyVal, regexPattern, RegexOptions.None, _regexTimeout);
        }
    }
}
