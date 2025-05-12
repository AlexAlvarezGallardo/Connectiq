using Connectiq.Contracts.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PersistenceWorker.Infrastructure.Configurations;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Address)
            .HasMaxLength(200);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(100);
    }
}