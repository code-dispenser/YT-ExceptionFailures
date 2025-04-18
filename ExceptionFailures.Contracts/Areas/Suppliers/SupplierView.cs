using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionFailures.Contracts.Areas.Suppliers;

public record class SupplierView(int SupplierID, string CompanyName, int ProductCount);


