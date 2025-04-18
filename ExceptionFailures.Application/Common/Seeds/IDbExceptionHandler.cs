using Flow.Core.Areas.Returns;

namespace ExceptionFailures.Application.Common.Seeds;

public interface IDbExceptionHandler
{
    public T Handle<T>(Exception ex);

    public Flow<T> HandleToFlow<T>(Exception ex);
}