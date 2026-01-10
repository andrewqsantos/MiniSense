using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniSense.Domain.Constants;
using MiniSense.Domain.Entities;

namespace MiniSense.Infrastructure.Configurations;

public class MeasurementUnitConfiguration : IEntityTypeConfiguration<MeasurementUnit>
{
    public void Configure(EntityTypeBuilder<MeasurementUnit> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Symbol)
            .HasMaxLength(ValidationConstants.MaxUnitSymbolLength)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(ValidationConstants.MaxUnitDescriptionLength)
            .IsRequired();
        
        builder.HasData(
            new { Id = 1, Symbol = "°C", Description = "Celsius" },
            new { Id = 2, Symbol = "mg/m³", Description = "Megagram per cubic metre" },
            new { Id = 3, Symbol = "hPa", Description = "Hectopascal" },
            new { Id = 4, Symbol = "lux", Description = "Lux" },
            new { Id = 5, Symbol = "%", Description = "Percent" }
        );
    }
}