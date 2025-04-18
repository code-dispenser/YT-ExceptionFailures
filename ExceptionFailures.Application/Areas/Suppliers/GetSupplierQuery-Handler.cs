using ExceptionFailures.Application.Common.Models.EFCore;
using ExceptionFailures.Application.Common.Seeds;
using ExceptionFailures.Contracts.Areas.Suppliers;
using Flow.Core.Areas.Returns;
using Flow.Core.Areas.Utilities;
using Instructor.Core.Common.Seeds;
using Microsoft.EntityFrameworkCore;

namespace ExceptionFailures.Application.Areas.Suppliers;

public record GetSupplierQuery(int SupplierID) : IInstruction<SupplierView> { }

public class GetSupplierQueryHandler(IDbContextReadOnly dbContext) : IInstructionHandler<GetSupplierQuery, SupplierView>
{
    public async Task<SupplierView> Handle(GetSupplierQuery instruction, CancellationToken cancellationToken)

        => await dbContext.Suppliers.Where(s => s.SupplierID == instruction.SupplierID).Select(Supplier.ProjectToSupplierView).SingleAsync();
}

#region Get with Flow

public record GetSupplierQueryWithFlow(int SupplierID) : IInstruction<Flow<SupplierView>> { }

public class GetSupplierQueryHandlerWithFlow(IDbContextReadOnly dbContext, IDbExceptionHandler exceptionHandler) : IInstructionHandler<GetSupplierQueryWithFlow, Flow<SupplierView>>
{
    public async Task<Flow<SupplierView>> Handle(GetSupplierQueryWithFlow instruction, CancellationToken cancellationToken)

        => await FlowHandler.TryToFlow
        (
            async () => await dbContext.Suppliers.Where(s => s.SupplierID == instruction.SupplierID).Select(Supplier.ProjectToSupplierView).SingleAsync(),
            (ex) => exceptionHandler.HandleToFlow<SupplierView>(ex)
        );
}

#endregion 