using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopzy.Domain.Entities;

namespace Shopzy.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : BaseConfiguration, IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);
    }
}