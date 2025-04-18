using ExceptionFailures.Application.Common.Seeds;
using ExceptionFailures.Application.Exceptions;
using Flow.Core.Areas.Returns;
using Microsoft.Data.Sqlite;

namespace ExceptionFailures.Infrastructure.Common.ExceptionHandlers;

public class SqlDbExceptionHandler : IDbExceptionHandler
{
    /*
        * We now have a handler specific to Sqlite exceptions, we can add more handlers classes or just functions for other boundaries, file IO exceptions, cloud service exceptions, network IO etc. 

        * However we are still dealing with Exceptions which in IMHO is not good, what we want is a return that can have either a success value or a failure values as currently we are
        * using exceptions for flow control instead of dealing with what is essentially a known failure. 
         
        * A real Exception (something Exceptional) will most likely not be something you can actually deal with, in which
        * case you will most likely just log it for investigation and show a message to the user if its possible to do so.
    */
    public T Handle<T>(Exception ex)
    {
        switch (ex)
        {
            case SqliteException sqlExecption when sqlExecption.SqliteErrorCode == 19:

                throw new DatabaseConstraintException("This action cannot be performed due to related items preventing it.", ex);

            default:
                
                throw new IDontKnowWhatHappenedException("An error occurred while processing your request.", ex);
        }
    }

    public Flow<T> HandleToFlow<T>(Exception ex)

        => ex switch
        {
            SqliteException sqliteEx when sqliteEx.SqliteErrorCode == 19 => new Failure.ConstraintFailure("This action cannot be performed due to related items preventing it."),
            SqliteException                                              => new Failure.ConnectionFailure("Unable to connect to the sqlite database"),
            _                                                            => Flow<T>.Failed(new Failure.UnknownFailure("A problem has occurred, please try again later", null, 0, true, ex))
        };
}