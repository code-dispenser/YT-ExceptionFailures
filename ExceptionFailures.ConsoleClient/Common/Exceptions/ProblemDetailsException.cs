using ExceptionFailures.ConsoleClient.Common.Models;

namespace ExceptionFailures.ConsoleClient.Common.Exceptions;

public class ProblemDetailsException(ProblemDetails problemDetails) : Exception
{
    public ProblemDetails ProblemDetails { get; } = problemDetails;
}
