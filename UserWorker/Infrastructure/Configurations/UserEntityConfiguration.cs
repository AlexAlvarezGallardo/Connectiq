using Connectiq.Contracts.User;
using Connectiq.GrpcUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                v => v.Select(s => Enum.Parse<Role>(s)).ToArray()            
            )
            .HasColumnType("text[]")
            .Metadata.SetValueComparer(new ValueComparer<Role[]>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToArray()
            ));

        builder.Property(c => c.EventId)
            .IsRequired();

        builder.Property(c => c.CreatedAt);

        builder.Property(c => c.IsActive);
    }
}