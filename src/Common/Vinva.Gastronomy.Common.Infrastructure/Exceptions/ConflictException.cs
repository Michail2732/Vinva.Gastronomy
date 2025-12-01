namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class ConflictException(string? message = null, Exception? innerException = null) : DomainException(message, innerException);