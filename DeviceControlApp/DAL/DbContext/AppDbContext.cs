using DeviceControlApp.DAL.EntitiesConfigurations;
using DeviceControlApp.Plugins.ExcelParser;
using Microsoft.EntityFrameworkCore;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp.DAL.DbContext;

public sealed class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{

    private readonly IExcelParser _excelParser;
    public AppDbContext(IExcelParser excelParser)
    {
        _excelParser = excelParser;
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
        var devicesFromExcel = _excelParser.ParseExcelFile("/storage/emulated/0/file.xlsx");
        modelBuilder.Entity<Device>().HasData(devicesFromExcel);
    }
}