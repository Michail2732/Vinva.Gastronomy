using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Identity.Domain.Exceptions
{
    public class IdentityDomainException : DomainException
    {                
        public IdentityDomainException(string message) : base(message) { }
        public IdentityDomainException(string message, Exception inner) : base(message, inner) { }        
    }
}
