using DeviceControlApp.DAL.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp.DAL.DbContext;

public sealed class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DbSet<Device> Devices { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "mydb.db");
        optionsBuilder.UseSqlite("DataSource=" + dbPath);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new DeviceConfiguration());
        // Добавляем начальные данные
        modelBuilder.Entity<Device>().HasData([
           new Device
           {
               Id = 1,
               Name = "Device1",
               Description = "Good Device",
               FactoryNumber = "BN1253632CD415",
               InventoryNumber = "153215215234623",
               Owner = "AGAT-SYSTEM",
               LastVerificationTime = DateTime.Now,
               NextVerificationTime = DateTime.Now
           },
           new Device
           {
               Id = 2,
               Name = "Device2",
               Description = "Excelent Device",
               FactoryNumber = "BL1GB56743",
               InventoryNumber = "5372367532KJCSA",
               Owner = "KAL-FACTORY",
               LastVerificationTime = DateTime.Now,
               NextVerificationTime = DateTime.Now
           }
        ]);
    }
}