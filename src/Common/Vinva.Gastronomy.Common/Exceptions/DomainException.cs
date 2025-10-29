using System;
using System.Collections.Generic;
using System.Text;

namespace Vinva.Gastronomy.Common.Exceptions
{
	[Serializable]
	public class DomainException : Exception
	{
		public DomainException() { }
		public DomainException(string message) : base(message) { }
		public DomainException(string message, Exception inner) : base(message, inner) { }		
	}
}
