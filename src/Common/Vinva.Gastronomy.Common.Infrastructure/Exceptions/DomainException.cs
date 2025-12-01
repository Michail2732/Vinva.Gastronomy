namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public abstract class DomainException(string? message = null, Exception? innerException = null) : Exception(message, innerException);