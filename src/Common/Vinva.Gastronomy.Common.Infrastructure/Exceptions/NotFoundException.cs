namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class NotFoundException(string? message = null, Exception? innerException = null) : DomainException(message, innerException);