using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;


public class BadRequestException : DomainException
{
	public BadRequestException(string? message = null, Exception? innerException = null)
		: base(message, innerException)
    {

	}

    public BadRequestException(Error error, Exception? innerException = null)
        : base($"{error.Code}.{error.Description}", innerException)
    {

    }
}
