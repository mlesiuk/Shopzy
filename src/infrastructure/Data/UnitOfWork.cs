using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Domain.Entities;
using Shopzy.Infrastructure.Persistence;

namespace Shopzy.Infrastructure.Data;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry<AuditableEntity>> entries = _context.ChangeTracker.Entries<AuditableEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(a => a.Created)
                    .CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(a => a.LastModified)
                    .CurrentValue = DateTime.UtcNow;
            }
        }

        return await _context.SaveChangesAsync(cancellationToken);
    }
}
