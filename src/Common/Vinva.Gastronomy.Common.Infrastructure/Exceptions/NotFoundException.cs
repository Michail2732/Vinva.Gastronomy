using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string? message = null, Exception? innerException = null) : base(message, innerException)
    {

    }

    public NotFoundException(Error error, Exception? innerException = null) : base(error, innerException)
    {

    }
}