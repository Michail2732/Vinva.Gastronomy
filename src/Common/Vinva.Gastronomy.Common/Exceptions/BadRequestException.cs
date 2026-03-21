using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Common.Exceptions;


public class BadRequestException : DomainException
{
	public BadRequestException(string? message = null, Exception? innerException = null)
		: base(message, innerException)
    {

	}

    public BadRequestException(Error error, Exception? innerException = null)
        : base(error, innerException)
    {

    }
}
