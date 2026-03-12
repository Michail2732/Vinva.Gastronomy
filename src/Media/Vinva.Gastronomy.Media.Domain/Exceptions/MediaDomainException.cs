using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Media.Domain.Exceptions
{

	[Serializable]
	public class MediaDomainException : DomainException
	{
		public MediaDomainException() { }
		public MediaDomainException(string message) : base(message) { }
		public MediaDomainException(string message, Exception inner) : base(message, inner) { }	
	}
}
