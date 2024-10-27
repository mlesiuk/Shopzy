using Microsoft.EntityFrameworkCore;

namespace Shopzy.Application.Data;

public interface IApplicationDbContext : IDisposable
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
