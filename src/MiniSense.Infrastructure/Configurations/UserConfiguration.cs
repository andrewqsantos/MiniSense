using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities;

namespace MiniSense.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username)
            .HasMaxLength(ValidationConstants.MaxUsernameLength)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(ValidationConstants.MaxEmailLength)
            .IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();
        
        builder.Metadata.FindNavigation(nameof(User.Devices))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}