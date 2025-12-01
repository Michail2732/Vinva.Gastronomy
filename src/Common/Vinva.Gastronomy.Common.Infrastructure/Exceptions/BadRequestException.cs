namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class BadRequestException(string? message = null, Exception? innerException = null) : DomainException(message, innerException);