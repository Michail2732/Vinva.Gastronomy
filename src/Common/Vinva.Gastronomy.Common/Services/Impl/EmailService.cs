using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Services
{
    public static class EmailService
    {
        public static bool Check(string email)
        {
            var regexQuery = "(^(?i)([a-z0-9-]+\\.)*[a-z0-9-]+@([a-z0-9-]+\\.)*[a-z0-9-]+\\.[a-z]{2,}+$)|(^$)";
            return Regex.IsMatch(email.ToLower(), regexQuery);
        }

    }
}
