using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp.DAL.EntitiesConfigurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("devices");
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();
        builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();
        builder.Property(e => e.Name).HasColumnName("Name").IsRequired();
    }
}