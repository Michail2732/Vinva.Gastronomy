namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class ForbiddenException(string? message = null, Exception? innerException = null) : DomainException(message, innerException);