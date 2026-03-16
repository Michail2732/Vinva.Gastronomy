using System;
using System.Collections.Generic;
using System.Text;
using Vinva.Gastronomy.Common.Entities;

namespace Vinva.Gastronomy.Common.Exceptions
{
	[Serializable]
	public class DomainException : Exception
	{
		public DomainException() { }
		public DomainException(string? message) : base(message) { }
		public DomainException(string? message, Exception? inner) : base(message, inner) { }
        public DomainException(Error error, Exception? inner) : base($"[{error.Code}] {error.Description}", inner) { }
    }
}
