using DeviceControlApp.DAL.Repositories;
using DeviceControlApp.Plugins.ExcelParser;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp.BLL;

public class DeviceService
{
    private readonly DeviceRepository _deviceRepository;

    public DeviceService(DeviceRepository deviceRepository, IExcelParser excelParser)
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
            "Название" => devices.Where(d => d.Name.Contains(searchText)).ToList(),
            "Фабричный номер" => devices.Where(d => d.FactoryNumber.Contains(searchText)).ToList(),
            "Инвентарный номер" => devices.Where(d => d.InventoryNumber.Contains(searchText)).ToList(),
            "Владелец" => devices.Where(d => d.Owner.Contains(searchText)).ToList(),
            "Описание" => devices.Where(d => d.Description != null && d.Description.Contains(searchText)).ToList(),
            _ => devices
        };

        var sortField = filterParameters.SortField;
        var isAscendingOrder = filterParameters.SortedByAscendingOrder;
        devices = sortField switch
        {
            "Название" => isAscendingOrder
                ? devices.OrderBy(d => d.Name).ToList()
                : devices.OrderByDescending(d => d.Name).ToList(),
            "Владелец" => isAscendingOrder
                ? devices.OrderBy(d => d.Owner).ToList()
                : devices.OrderByDescending(d => d.Owner).ToList(),
            "Дата последней поверки" => isAscendingOrder
                ? devices.OrderBy(d => d.LastVerificationTime).ToList()
                : devices.OrderByDescending(d => d.LastVerificationTime).ToList(),
            "Дата следующей поверки" => isAscendingOrder
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
            "Название" => filterParameters.SortedByAscendingOrder
                ? devices.OrderBy(d => d.Name).ToList()
                : devices.OrderByDescending(d => d.Name).ToList(),
            "Владелец" => filterParameters.SortedByAscendingOrder
                ? devices.OrderBy(d => d.Owner).ToList()
                : devices.OrderByDescending(d => d.Owner).ToList(),
            "Дата последней поверки" => filterParameters.SortedByAscendingOrder
                ? devices.OrderBy(d => d.LastVerificationTime).ToList()
                : devices.OrderByDescending(d => d.LastVerificationTime).ToList(),
            "Дата следующей поверки" => filterParameters.SortedByAscendingOrder
                ? devices.OrderBy(d => d.NextVerificationTime).ToList()
                : devices.OrderByDescending(d => d.NextVerificationTime).ToList(),
            _ => devices
        };
        return devices;
    }

    public async Task<IEnumerable<Device>> GetScheduledDevices(DateTime schedulePeriod)
    {
        var devices = await _deviceRepository.GetAllDevicesAsync();
        devices = devices.Where(d => d.NextVerificationTime <= schedulePeriod).ToList();
        return devices;
    }
  
}