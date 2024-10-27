using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopzy.Domain.Entities;

namespace Shopzy.Infrastructure.Persistence.Configurations;

public class CartConfiguration : BaseConfiguration, IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.UserId);

        builder
            .HasMany(c => c.CartItems)
            .WithOne(ci => ci.Cart);
    }
}
