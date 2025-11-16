using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.Domain.Constants
{
    public class IdentityErrorMessages
    {
        public static string NewEmailIsIncorrect(string newEmail) => $"Email '{newEmail}' некорректный";
    }
}
