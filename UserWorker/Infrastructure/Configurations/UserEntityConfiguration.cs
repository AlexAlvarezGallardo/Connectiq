using Connectiq.Contracts.User;
using Connectiq.GrpcUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserWorker.Infrastructure.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.PasswordHash)
            .HasMaxLength(200);

        builder.Property(c => c.Roles)
            .HasConversion(
                v => v.Select(r => r.ToString()).ToArray(),
                v => v.Select(s => Enum.Parse<Role>(s)).ToList()
            )
            .HasColumnType("text[]");

        builder.Property(c => c.EventId)
            .IsRequired();

        builder.Property(c => c.CreatedAt);

        builder.Property(c => c.IsActive);
    }
}