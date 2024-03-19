using System.Collections.ObjectModel;
using System.Diagnostics;
using DeviceControlApp.BLL;
using DeviceControlApp.DAL.Repositories;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp;

public partial class MainPage : ContentPage
{

    private bool _isScheduleMode = false;
    public string Mode { get; set; }
    private DateTime _schedulePeriod;
    private FilterParameters _filterParameters;
    private readonly DeviceService _deviceService;
    public static ObservableCollection<Device> Devices { get; set; } = [];
    
    public MainPage()
    {
        Mode = "Режим просмотра";
        _schedulePeriod = DateTime.Now.AddDays(1);
        _filterParameters = new FilterParameters();
        _deviceService = MauiProgram.Services!.GetService<DeviceService>()!;
        InitializeComponent();
        BindingContext = this;
    }
    
    private async void AddButton_Clicked(object? sender, EventArgs e)
    {
        if(!_isScheduleMode)
            await Navigation.PushModalAsync(new AddDevicePage());
        else
        {
            await DisplayAlert("Ошибка", "Перйдите в режим просмотра", "OK");
        }
    }

    protected override async  void OnAppearing()
    {
        base.OnAppearing();
        var devices = await _deviceService.GetOrderedDevices(_filterParameters);
        if (_isScheduleMode)
        {
            devices = devices.Where(d => d.NextVerificationTime <= _schedulePeriod).ToList();
        }
        foreach (var device in devices)
        {
            Devices.Add(device);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Devices.Clear();
    }

    private async void InputView_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var searchText = e.NewTextValue;
        var filteredDevices = await _deviceService.GetDevicesByFilterAsync(searchText, _filterParameters);
        if (_isScheduleMode)
        {
            filteredDevices = filteredDevices.Where(d => d.NextVerificationTime <= _schedulePeriod).ToList();
        }
        Devices.Clear();
        foreach (var device in filteredDevices)
        {
            Devices.Add(device);
        }
    }

    private async void EditButton_Clicked(object? sender, EventArgs e)
    {
        if (_isScheduleMode)
        {
            await DisplayAlert("Ошибка", "Перейдите в режим просмотра", "OK");
        }
        else
        {
            var button = (Button)sender!;
            var item = button.BindingContext as Device;
            await Navigation.PushModalAsync(new EditDevicePage(item!));
        }
    }

    private async void DeleteButton_Clicked(object? sender, EventArgs e)
    {
        if (_isScheduleMode)
        {
            await DisplayAlert("Ошибка", "Перейдите в режим просмотра", "OK");
        }
        else
        {
            var button = (Button)sender!;
            var item = button.BindingContext as Device;
            await _deviceService.DeleteDevice(item!);
            Devices.Remove(item!);
        }
    }

    private async void FiltersButton_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new FiltersPage(_filterParameters,this));
    }

    public void ApplyFilters(FilterParameters newFilterParameters)
    {
        _filterParameters = newFilterParameters;
    }

    private async void ScheduleButton_Clicked(object? sender, EventArgs e)
    {
        var headerLabel = this.FindByName<Label>("HeaderLabel");
        if (!_isScheduleMode)
        {
            Devices.Clear();
            var scheduledDevices = await _deviceService.GetScheduledDevices(_schedulePeriod);
            foreach (var device in scheduledDevices)
            {
                Devices.Add(device);
            }
            _isScheduleMode = true;
            headerLabel.Text = "Режим планирования";
        }
        else
        {
            Devices.Clear();
            var devices = await _deviceService.GetOrderedDevices(_filterParameters);
            foreach (var device in devices)
            {
                Devices.Add(device);
            }
            _isScheduleMode = false;
            headerLabel.Text = "Режим просмотра";
        }
    }

    private void TimePeriodPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (sender is not Picker picker) return;
        _schedulePeriod = picker.SelectedItem switch
        {
            "1 день" => DateTime.Now.AddDays(1),
            "1 неделя" => DateTime.Now.AddDays(7),
            "1 месяц" => DateTime.Now.AddMonths(1),
            _ => _schedulePeriod
        };
        _isScheduleMode = false;
    }
}