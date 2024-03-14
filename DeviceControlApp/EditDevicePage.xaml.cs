using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DeviceControlApp.BLL;
using DeviceControlApp.BLL.DTO;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp;

public partial class EditDevicePage : ContentPage
{

    private readonly DeviceService _deviceService;

    private readonly IMapper _mapper;
    public DeviceEditDto DeviceEditDto { get; set; }
    
    private Device _deviceToEdit;
    
    public EditDevicePage(Device deviceToEdit)
    {
        _deviceToEdit = deviceToEdit;
        _mapper = MauiProgram.Services!.GetService<IMapper>()!;
        _deviceService = MauiProgram.Services!.GetService<DeviceService>()!;
        DeviceEditDto = _mapper.Map<Device, DeviceEditDto>(_deviceToEdit);
        BindingContext = this;
        InitializeComponent();
    }

    private async void SaveButton_Clicked(object? sender, EventArgs e)
    {
        if (Validator.TryValidateObject(DeviceEditDto, new ValidationContext(DeviceEditDto), null, true))
        {
            var updatedDevice = _mapper.Map<DeviceEditDto, Device>(DeviceEditDto);
            updatedDevice.Id = _deviceToEdit.Id;
            await _deviceService.UpdateDeviceAsync(updatedDevice);
            await Navigation.PopModalAsync();
        }
        else
        {
            foreach (var validationResult in GetValidationResults(DeviceEditDto))
            {
                await DisplayAlert("Error", validationResult.ErrorMessage, "OK");
            }
        }
    }

    private async void BackButton_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    
    private List<ValidationResult> GetValidationResults(object obj)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(obj);
        Validator.TryValidateObject(obj, context, validationResults, true);
        return validationResults;
    }
}