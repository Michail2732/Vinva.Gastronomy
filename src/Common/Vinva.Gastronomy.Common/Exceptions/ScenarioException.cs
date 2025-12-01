using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Exceptions
{    
    [Serializable]
    public class ScenarioException : DomainException
    {
        public ScenarioException() { }
        public ScenarioException(string message) : base(message) { }
        public ScenarioException(string message, Exception inner) : base(message, inner) { }
    }
}
