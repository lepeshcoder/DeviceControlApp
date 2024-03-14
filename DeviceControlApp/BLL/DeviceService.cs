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

        var searchField = filterParameters.SearchField;
        devices = searchField switch
        {
            "Name" => devices.Where(d => d.Name.Contains(searchText)).ToList(),
            "FactoryNumber" => devices.Where(d => d.FactoryNumber.Contains(searchText)).ToList(),
            "InventoryNumber" => devices.Where(d => d.InventoryNumber.Contains(searchText)).ToList(),
            "Owner" => devices.Where(d => d.Owner.Contains(searchText)).ToList(),
            "Description" => devices.Where(d => d.Description != null && d.Description.Contains(searchText)).ToList(),
            _ => devices
        };

        var sortField = filterParameters.SortField;
        var isAscendingOrder = filterParameters.SortedByAscendingOrder;
        devices = sortField switch
        {
            "Name" => isAscendingOrder
                ? devices.OrderBy(d => d.Name).ToList()
                : devices.OrderByDescending(d => d.Name).ToList(),
            "Owner" => isAscendingOrder
                ? devices.OrderBy(d => d.Owner).ToList()
                : devices.OrderByDescending(d => d.Owner).ToList(),
            "LastVerificationTime" => isAscendingOrder
                ? devices.OrderBy(d => d.LastVerificationTime).ToList()
                : devices.OrderByDescending(d => d.LastVerificationTime).ToList(),
            "NextVerificationTime" => isAscendingOrder
                ? devices.OrderBy(d => d.NextVerificationTime).ToList()
                : devices.OrderByDescending(d => d.NextVerificationTime).ToList(),
            _ => devices
        };

        return devices;
    }

    public async Task<List<Device>> GetOrderedDevices(FilterParameters filterParameters)
    {
        var devices = await _deviceRepository.GetAllDevicesAsync();
        devices = filterParameters.SortField switch
        {
            "Name" => filterParameters.SortedByAscendingOrder
                ? devices.OrderBy(d => d.Name).ToList()
                : devices.OrderByDescending(d => d.Name).ToList(),
            "Owner" => filterParameters.SortedByAscendingOrder
                ? devices.OrderBy(d => d.Owner).ToList()
                : devices.OrderByDescending(d => d.Owner).ToList(),
            "LastVerificationTime" => filterParameters.SortedByAscendingOrder
                ? devices.OrderBy(d => d.LastVerificationTime).ToList()
                : devices.OrderByDescending(d => d.LastVerificationTime).ToList(),
            "NextVerificationTime" => filterParameters.SortedByAscendingOrder
                ? devices.OrderBy(d => d.NextVerificationTime).ToList()
                : devices.OrderByDescending(d => d.NextVerificationTime).ToList(),
            _ => devices
        };
        return devices;
    }
}