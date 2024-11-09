using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Shopzy.Domain.Entities;
using Shopzy.Application.Abstractions.Interfaces;

namespace Shopzy.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    private readonly DateTime _dateTime = DateTime.UtcNow;
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var username = _currentUserService.UserName;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry is null)
            {
                continue;
            }

            if (entry.Entity is null)
            {
                continue;
            }

            if (string.IsNullOrEmpty(username) && entry.Entity.GetType().Equals(typeof(User)))
            {
                username = (entry.Entity as User)?.Username ?? "system";
            }

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = username;
                    entry.Entity.CreatedUtc = _dateTime;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = username;
                    entry.Entity.LastModifiedUtc = _dateTime;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
