using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string? message = null, Exception? innerException = null) : base(message, innerException)
    {

    }

    public UnauthorizedException(Error error, Exception? innerException = null) : base($"{error.Code}.{error.Description}", innerException)
    {

    }
}