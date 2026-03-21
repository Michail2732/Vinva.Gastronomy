using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Common.Exceptions;

public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string? message = null, Exception? innerException = null) : base(message, innerException)
    {

    }

    public UnauthorizedException(Error error, Exception? innerException = null) : base(error, innerException)
    {

    }
}