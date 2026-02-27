using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class ConflictException : DomainException
{
    public ConflictException(string? message = null, Exception? innerException = null) : base(message, innerException)
    {

    }

    public ConflictException(Error error, Exception? innerException = null) : base(error, innerException)
    {

    }
}
