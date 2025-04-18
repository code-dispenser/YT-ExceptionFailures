using ExceptionFailures.Application.Common.Models.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionFailures.Application.Common.Seeds;

public  interface IDbContextReadOnly
{
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Product> Products   { get; set; }
}
