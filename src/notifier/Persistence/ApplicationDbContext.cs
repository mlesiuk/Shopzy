using Microsoft.EntityFrameworkCore;
using Shopzy.Notifier.Models;
using System.Reflection;

namespace Shopzy.Notifier.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<OutgoingMessage> OutgoingMessages { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
