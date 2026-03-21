using System;
using System.Collections.Generic;
using System.Text;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Recipes.Domain.Exceptions
{
	[Serializable]
	public class RecipeDomainException : DomainException
	{		
		public RecipeDomainException(Type type, string message, Exception? inner = null) : base(type, message, inner) { }
		public RecipeDomainException(string message, Exception inner) : base(message, inner) { }		
	}
}
