using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class ForbiddenException : DomainException
{
    public ForbiddenException(string? message = null, Exception? innerException = null) : base(message, innerException)
    {

    }

    public ForbiddenException(Error error, Exception? innerException = null) : base(error, innerException)
    {

    }
}