using System.Linq.Expressions;
using DeviceControlApp.DAL.DbContext;
using Microsoft.EntityFrameworkCore;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp.DAL.Repositories;

public class DeviceRepository
{
    private readonly AppDbContext _dbContext;

    public DeviceRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Device>> GetAllDevicesAsync()
    {
        return await _dbContext.Devices.ToListAsync();
    }

    public async Task<Device> AddDeviceAsync(Device device)
    {
        return (await _dbContext.Devices.AddAsync(device)).Entity;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Device>> GetDevicesByFilter(Expression<Func<Device, bool>> predicate)
    {
        var filteredDevices = _dbContext.Devices.Where(predicate);
        return await filteredDevices.ToListAsync();
    }

    public async Task<Device> UpdateDeviceAsync(Device updatedDevice)
    {
        var itemToUpdate = _dbContext.Devices.Find(updatedDevice.Id);
        DeleteDevice(itemToUpdate!);
        await AddDeviceAsync(updatedDevice);
        return updatedDevice;
    }

    public Device DeleteDevice(Device item)
    { 
        _dbContext.Devices.Remove(item);
        return item;
    }

    public List<Device> GetAllDevices()
    {
        return _dbContext.Devices.ToList();
    }
}