using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopzy.Domain.Entities;

namespace Shopzy.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : BaseConfiguration, IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .HasMany(r => r.Users)
            .WithMany(u => u.Roles);
    }
}
