using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopzy.Domain.Entities;

namespace Shopzy.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseConfiguration, IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder
            .HasKey(x => x.Id);

        builder
            .Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .HasIndex(u => u.Username)
            .IsUnique();

        builder
            .Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .HasIndex(u => u.Email)
            .IsUnique();

        builder
            .Property(u => u.Name)
            .HasMaxLength(255);

        builder
            .Property(u => u.SurName)
            .HasMaxLength(255);

        builder
            .OwnsMany(u => u.Addressess);
    }
}
