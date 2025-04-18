using Flow.Core.Areas.Returns;
using Flow.Core.Common.Models;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace ExceptionFailures.Contracts.Areas.Suppliers;

[Service]
public interface IGrpcSuppliersService
{
    Task<Flow<GetSupplierResponse>> GetSupplier(GetSupplier instruction, CallContext context = default);

    Task<Flow<None>> DeleteSupplier(DeleteSupplier instruction, CallContext context = default);
}
