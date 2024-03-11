using System.Linq.Expressions;
using DeviceControlApp.DAL.Repositories;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp.BLL;

public class DeviceService
{
    private readonly DeviceRepository _deviceRepository;

    public DeviceService(DeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public List<Device> GetAllDevices()
    {
        return _deviceRepository.GetAllDevices();
    }

    public async Task<List<Device>> GetAllDevicesAsync()
    {
        return await _deviceRepository.GetAllDevicesAsync();
    }

    public async Task<Device> AddDevice(Device device)
    {
        var newDevice = await _deviceRepository.AddDeviceAsync(device);
        await _deviceRepository.SaveChangesAsync();
        return newDevice;
    }

    public async Task<List<Device>> GetDevicesByName(string searchText)
    {
        return await _deviceRepository.GetDevicesByFilter(device => device.Name.Contains(searchText));
    }

    public async Task<Device> UpdateDeviceAsync(Device updatedDevice)
    {
       var item = await _deviceRepository.UpdateDeviceAsync(updatedDevice);
       await _deviceRepository.SaveChangesAsync();
       return item;
    }

    public async Task<Device> DeleteDevice(Device item)
    {
       var deleteDevice = _deviceRepository.DeleteDevice(item);
       await _deviceRepository.SaveChangesAsync();
       return deleteDevice;
    }

    public async Task<List<Device>> GetDevicesByFilterAsync(string searchText, FilterParameters filterParameters)
    {
        var devices = await _deviceRepository.GetAllDevicesAsync();

        var searchField = typeof(Device).GetProperty(filterParameters.SearchField);
        devices = devices.Where(device => ((string)searchField!.GetValue(device)!).Contains(searchText) ).ToList();
        
        var sortField =  typeof(Device).GetProperty(filterParameters.SortField);
        devices = filterParameters.SortedByAscendingOrder ?
            devices.OrderBy(device => sortField.GetValue(device)).ToList() : 
            devices.OrderByDescending(device => sortField.GetValue(device)).ToList();
        
        return devices;
    }
}