using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Identity.Domain.Exceptions
{
    public class RegistrationException : IdentityDomainException
    {
        public RegistrationException(Type type) : base(type) { }
        public RegistrationException(Type type, string message) : base(type, message) { }
        public RegistrationException(Type type, string message, Exception inner) : base(type, message, inner) { }
    }
}
