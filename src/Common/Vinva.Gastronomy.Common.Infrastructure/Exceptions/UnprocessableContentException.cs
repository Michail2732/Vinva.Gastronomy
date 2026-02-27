using Vinva.Gastronomy.Common.Infrastructure.Results;

namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class UnprocessableContentException : DomainException
{
    public UnprocessableContentException(string? message = null, Exception? innerException = null) : base(message, innerException)
    {

    }

    public UnprocessableContentException(Error error, Exception? innerException = null) : base(error, innerException)
    {

    }
}
