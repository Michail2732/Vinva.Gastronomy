namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class UnauthorizedException(string? message = null, Exception? innerException = null) : DomainException(message, innerException);