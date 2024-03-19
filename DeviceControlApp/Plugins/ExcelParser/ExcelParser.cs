using System.Diagnostics;
using OfficeOpenXml;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp.Plugins.ExcelParser;

public class ExcelParser : IExcelParser
{
    public IEnumerable<Device> ParseExcelFile(string fileAbsolutePath)
    {
        var devices = new List<Device>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (var package = new ExcelPackage(new FileInfo(fileAbsolutePath)))
        {
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                throw new Exception("Failed to load excel file");
            }
            
            var rowCount = worksheet.Dimension.Rows;
            var colCount = worksheet.Dimension.Columns;
            uint id = 1;
            for (var row = 3; row <= rowCount; row++) // assuming your data starts from the second row
            {
                var name = worksheet.Cells[row, 2]?.Value?.ToString() ?? "not set";
                var factoryNumber = worksheet.Cells[row, 4]?.Value?.ToString() ?? "not set";
                var inventoryNumber = worksheet.Cells[row, 5]?.Value?.ToString() ?? "not set";
                var success = DateTime.TryParse(worksheet.Cells[row, 9]?.Value?.ToString(), out var nextVerificationTime);
                if (!success) nextVerificationTime = DateTime.MinValue;
                var owner = worksheet.Cells[row, 10]?.Value?.ToString() ?? "not set";
                var device = new Device
                {
                    Id = id++,
                    Name = name,
                    FactoryNumber = factoryNumber,
                    InventoryNumber = inventoryNumber,
                    Owner = owner,
                    NextVerificationTime = nextVerificationTime
                };
                devices.Add(device);
            }
        }
        return devices;
    }
}