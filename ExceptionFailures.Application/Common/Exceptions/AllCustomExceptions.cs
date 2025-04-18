namespace ExceptionFailures.Application.Exceptions;

public class DatabaseConstraintException(string message, Exception? innerException) : Exception(message, innerException);

public class IDontKnowWhatHappenedException(string message, Exception? innerException) : Exception(message, innerException);
