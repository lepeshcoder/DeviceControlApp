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
            new Device{Id = 1,Name = "kal1"},
            new Device{Id = 2,Name = "kal2"},
            new Device{Id = 3,Name = "kal3"}
        ]);
    }
}