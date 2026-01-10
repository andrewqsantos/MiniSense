using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities;

namespace MiniSense.Infrastructure.Configurations;

public class DataStreamConfiguration : IEntityTypeConfiguration<DataStream>
{
    public void Configure(EntityTypeBuilder<DataStream> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Key).IsRequired();
        builder.HasIndex(x => x.Key).IsUnique();

        builder.Property(x => x.Label)
            .HasMaxLength(ValidationConstants.MaxStreamLabelLength)
            .IsRequired();

        builder.HasOne(x => x.MeasurementUnit)
            .WithMany(x => x.Streams)
            .HasForeignKey(x => x.MeasurementUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Measurements)
            .WithOne(x => x.DataStream)
            .HasForeignKey(x => x.DataStreamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}