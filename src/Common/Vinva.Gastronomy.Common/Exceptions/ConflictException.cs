using System;
using Vinva.Gastronomy.Common.Entities;
using Vinva.Gastronomy.Common.Exceptions;

namespace Vinva.Gastronomy.Common.Exceptions;

public class ConflictException : DomainException
{
    public ConflictException(string? message = null, Exception? innerException = null) : base(message, innerException)
    {

    }

    public ConflictException(Error error, Exception? innerException = null) : base(error, innerException)
    {

    }
}
