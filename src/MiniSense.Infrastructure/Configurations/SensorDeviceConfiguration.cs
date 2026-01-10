using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities;

namespace MiniSense.Infrastructure.Configurations;

public class SensorDeviceConfiguration : IEntityTypeConfiguration<SensorDevice>
{
    public void Configure(EntityTypeBuilder<SensorDevice> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Key)
            .IsRequired();
        
        builder.HasIndex(x => x.Key)
            .IsUnique();
        
        builder.Property(x => x.Label)
            .HasMaxLength(ValidationConstants.MaxLabelLength)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(ValidationConstants.MaxDescriptionLength);
        
        builder.HasMany(x => x.Streams)
            .WithOne(x => x.SensorDevice)
            .HasForeignKey(x => x.SensorDeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
