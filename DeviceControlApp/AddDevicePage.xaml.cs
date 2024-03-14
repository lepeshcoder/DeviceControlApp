using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceControlApp.BLL;
using DeviceControlApp.BLL.DTO;
using DeviceControlApp.DAL.Repositories;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp;

public partial class AddDevicePage : ContentPage
{
    private readonly DeviceService _deviceService;
    public DeviceAddDto DeviceAddDto { get; set; }
    
    public AddDevicePage()
    {
        _deviceService = MauiProgram.Services!.GetService<DeviceService>()!;
        InitializeComponent();
        DeviceAddDto = new DeviceAddDto(); 
        BindingContext = this; 
    }

    private async void SaveButton_Clicked(object? sender, EventArgs e)
    {
        if (Validator.TryValidateObject(DeviceAddDto, new ValidationContext(DeviceAddDto), null, true))
        {
            var newDevice = new Device
            {
                Name = DeviceAddDto.Name,
                Description = DeviceAddDto.Description,
                FactoryNumber = DeviceAddDto.FactoryNumber,
                InventoryNumber = DeviceAddDto.InventoryNumber,
                Owner = DeviceAddDto.Owner,
                LastVerificationTime = DeviceAddDto.LastVerificationTime,
                NextVerificationTime = DeviceAddDto.NextVerificationTime
            };
            await _deviceService.AddDevice(newDevice);
            await Navigation.PopModalAsync();
        }
        else
        {
            foreach (var validationResult in GetValidationResults(DeviceAddDto))
            {
                await DisplayAlert("Error", validationResult.ErrorMessage, "OK");
            }
        }
    }

    private async void BackButton_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    
    // Метод для получения результатов валидации
    private List<ValidationResult> GetValidationResults(object obj)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(obj);
        Validator.TryValidateObject(obj, context, validationResults, true);
        return validationResults;
    }
}