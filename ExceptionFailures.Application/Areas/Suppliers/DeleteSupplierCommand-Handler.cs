using ExceptionFailures.Application.Common.Seeds;
using ExceptionFailures.Application.Exceptions;
using Flow.Core.Areas.Returns;
using Flow.Core.Areas.Utilities;
using Flow.Core.Common.Models;
using Instructor.Core.Common.Seeds;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExceptionFailures.Application.Areas.Suppliers;

public record DeleteSupplierCommand(int supplierID) : IInstruction<None> { }

//public class DeleteSupplierCommandHandler(IDbContextWrite dbContext) : IInstructionHandler<DeleteSupplierCommand, None>
//{
//    //public async Task<None> Handle(DeleteSupplierCommand instruction, CancellationToken cancellationToken)
//    //{
//    //    await dbContext.Suppliers.Where(x => x.SupplierID == instruction.supplierID).ExecuteDeleteAsync(cancellationToken);

//    //    return None.Value;
//    //}

//    #region First Refactor
//    public async Task<None> Handle(DeleteSupplierCommand instruction, CancellationToken cancellationToken)
//    {
//        try
//        {
//            await dbContext.Suppliers.Where(x => x.SupplierID == instruction.supplierID).ExecuteDeleteAsync(cancellationToken);
//        }
//        /*
//            * Now what, log, throw, return None as if nothing happened? maybe throw a custom exception? 
//            * How many times are you going to have to do this, every method that talks to your database?
//        */
//        catch (SqliteException ex) when (ex.SqliteErrorCode == 19)// Or use a nuget package that saves you doing this but not much else!
//        {
//            throw new DatabaseConstraintException("You need to make sure the Supplier does not have products in the system before you can delete them.", ex);
//        }
//        catch (DbUpdateException) { throw; }
//        catch { throw; }

//        return None.Value;
//    }
//    #endregion

//}

public static class ErrorHandler
{
    public static async Task<T> TryCatch<T>(Func<Task<T>> operationToTry, Func<Exception, T> exceptionHandler)
    {
        try
        {
            return await operationToTry();
        }
        catch(Exception ex) { return exceptionHandler(ex); }
    }
}


#region Second Refactor
public class DeleteSupplierCommandHandler(IDbContextWrite dbContext, IDbExceptionHandler dbExceptionHandler) : IInstructionHandler<DeleteSupplierCommand, None>
{
    public async Task<None> Handle(DeleteSupplierCommand instruction, CancellationToken cancellationToken)

        => await ErrorHandler.TryCatch(
            async () =>
            {
                await dbContext.Suppliers.Where(x => x.SupplierID == instruction.supplierID).ExecuteDeleteAsync(cancellationToken);
                return None.Value;
            },
            ex => dbExceptionHandler.Handle<None>(ex)
        );

}

#endregion

#region Using a result type  like Flow

public record DeleteSupplierCommandWithFlow(int supplierID) : IInstruction<Flow<None>> { }

public class DeleteSupplierCommandHandlerWithFlow(IDbContextWrite dbContext, IDbExceptionHandler dbExceptionHandler) : IInstructionHandler<DeleteSupplierCommandWithFlow, Flow<None>>
{
    public async Task<Flow<None>> Handle(DeleteSupplierCommandWithFlow instruction, CancellationToken cancellationToken)

        => await FlowHandler.TryToFlow(
            async () =>
            {
                await dbContext.Suppliers.Where(x => x.SupplierID == instruction.supplierID).ExecuteDeleteAsync(cancellationToken);
                return None.Value;
            },
            ex => dbExceptionHandler.HandleToFlow<None>(ex)
        );

}

#endregion region