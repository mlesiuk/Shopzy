using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopzy.Notifier.Models;

namespace Shopzy.Notifier.Persistence.Configurations;

public class OutgoingMessageConfiguration : IEntityTypeConfiguration<OutgoingMessage>
{
    public void Configure(EntityTypeBuilder<OutgoingMessage> builder)
    {
        builder
            .Property(pe => pe.Id)
            .IsRequired();

        builder
            .HasKey(pe => pe.Id);

        builder
            .Property(pe => pe.Content)
            .IsRequired()
            .HasMaxLength(8000);
    }
}
