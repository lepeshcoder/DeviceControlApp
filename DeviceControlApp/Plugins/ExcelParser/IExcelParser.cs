using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp.Plugins.ExcelParser;

public interface IExcelParser
{
    IEnumerable<Device> ParseExcelFile(string fileAbsolutePath);
}