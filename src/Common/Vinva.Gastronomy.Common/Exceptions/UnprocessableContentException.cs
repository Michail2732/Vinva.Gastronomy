using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Common.Exceptions;

public class UnprocessableContentException : DomainException
{
    public UnprocessableContentException(string? message = null, Exception? innerException = null) : base(message, innerException)
    {

    }

    public UnprocessableContentException(Error error, Exception? innerException = null) : base(error, innerException)
    {

    }
}
