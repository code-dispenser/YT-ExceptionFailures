using ExceptionFailures.Application.Common.Models.EFCore;
using Microsoft.EntityFrameworkCore;

namespace ExceptionFailures.Application.Common.Seeds
{
    public interface IDbContextWrite
    {
        public DbSet<Supplier> Suppliers { get; set; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
