using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public abstract class DomainException : Exception
{
    public DomainException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {

    }

    public DomainException(Error error, Exception? innerException = null)
        : base($"{error.Code}.{error.Description}", innerException)
    {

    }
}