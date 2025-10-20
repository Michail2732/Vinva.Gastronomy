using System;
using System.Collections.Generic;
using System.Text;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Recipes.Domain.Exceptions
{
	[Serializable]
	public class RecipeDomainException : Exception
	{
		public RecipeDomainException() { }
		public RecipeDomainException(string message) : base(message) { }
		public RecipeDomainException(string message, Exception inner) : base(message, inner) { }
		protected RecipeDomainException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
