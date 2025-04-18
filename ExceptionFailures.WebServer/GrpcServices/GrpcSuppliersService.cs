using ExceptionFailures.Application.Areas.Suppliers;
using ExceptionFailures.Contracts.Areas.Suppliers;
using Flow.Core.Areas.Extensions;
using Flow.Core.Areas.Returns;
using Flow.Core.Common.Models;
using Instructor.Core.Common.Seeds;
using ProtoBuf.Grpc;

namespace ExceptionFailures.WebServer.GrpcServices;

public class GrpcSuppliersService(IInstructionDispatcher instructionDispatcher) : IGrpcSuppliersService
{
    private readonly IInstructionDispatcher _instructionDispatcher = instructionDispatcher;

    public async Task<Flow<GetSupplierResponse>> GetSupplier(GetSupplier instruction, CallContext context = default)

        => await _instructionDispatcher
                    .SendInstruction(new GetSupplierQueryWithFlow(instruction.SupplierID), context.CancellationToken)
                        .ReturnAs(failure => failure, success => new GetSupplierResponse(success));

    public async Task<Flow<None>> DeleteSupplier(DeleteSupplier instruction, CallContext context = default)

        => await _instructionDispatcher.SendInstruction(new DeleteSupplierCommandWithFlow(instruction.SupplierID), context.CancellationToken);
    
    /*
        * Or we could do other stuff with flows extensions OnSuccess, OnFailure such as check the failure type, provide better user information if necessary, call other methods, retry the method whatever you want, but very easily. 
    */


    
}