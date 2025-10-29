using System;
using System.Collections.Generic;
using System.Text;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Recipes.Domain.Exceptions
{
	[Serializable]
	public class RecipeDomainException : EntityDomainException
	{
		public RecipeDomainException(Type type) : base(type) { }
		public RecipeDomainException(Type type, string message) : base(type, message) { }
		public RecipeDomainException(Type type, string message, Exception inner) : base(type, message, inner) { }		
	}
}
