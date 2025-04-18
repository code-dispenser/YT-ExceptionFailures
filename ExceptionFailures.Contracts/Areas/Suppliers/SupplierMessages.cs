using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionFailures.Contracts.Areas.Suppliers;

/*
    * Using newer protobuf-net allows c# records without attributes i.e [ProtoContract], [ProtoMember]
*/
public record GetSupplier(int SupplierID);

public record GetSupplierResponse(SupplierView SupplierView);

public record DeleteSupplier(int SupplierID);
