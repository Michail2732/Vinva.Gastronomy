using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Exceptions;

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