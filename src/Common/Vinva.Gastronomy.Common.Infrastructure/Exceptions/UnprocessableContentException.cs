namespace Vinva.Gastronomy.Common.Infrastructure.Exceptions;

public class UnprocessableContentException(string? message = null, Exception? innerException = null) : DomainException(message, innerException);