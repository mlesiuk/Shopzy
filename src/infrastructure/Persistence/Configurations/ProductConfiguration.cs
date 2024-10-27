using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopzy.Domain.Entities;

namespace Shopzy.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : BaseConfiguration, IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .OwnsOne(product => product.Price, moneyBuilder =>
            {
                moneyBuilder
                    .Property(money => money.Currency)
                    .HasMaxLength(3);

                moneyBuilder
                    .Property(money => money.Amount).
                    HasPrecision(18, 2);
            });
            
    }
}