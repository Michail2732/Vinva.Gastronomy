using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Identity.Domain.Exceptions
{
    public class IdentityDomainException : EntityDomainException
    {        
        public IdentityDomainException(Type type) : base(type) { }
        public IdentityDomainException(Type type, string message) : base(type, message) { }
        public IdentityDomainException(Type type, string message, Exception inner) : base(type, message, inner) { }
    }
}
