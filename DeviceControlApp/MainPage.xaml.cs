using System.Collections.ObjectModel;
using DeviceControlApp.BLL;
using DeviceControlApp.DAL.Repositories;
using Device = DeviceControlApp.DAL.Entities.Device;

namespace DeviceControlApp;

public partial class MainPage : ContentPage
{

    private FilterParameters _filterParameters;
    private readonly DeviceService _deviceService;
    public ObservableCollection<Device> Devices { get; set; } = [];
    
    public MainPage()
    {
        _filterParameters = new FilterParameters();
        _deviceService = MauiProgram.Services!.GetService<DeviceService>()!;
        InitializeComponent();
        BindingContext = this;
    }
    
    private async void AddButton_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AddDevicePage());
    }

    protected override async  void OnAppearing()
    {
        base.OnAppearing();
        var devices = await _deviceService.GetOrderedDevices(_filterParameters);
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
        Devices.Clear();
        var devicesBySearchText = await _deviceService.GetDevicesByFilterAsync(searchText,_filterParameters);
        foreach (var device in devicesBySearchText)
        {
            Devices.Add(device);
        }
    }

    private async void EditButton_Clicked(object? sender, EventArgs e)
    {
        var button = (Button)sender!;
        var item = button.BindingContext as Device;
        await Navigation.PushModalAsync(new EditDevicePage(item!));
    }

    private async void DeleteButton_Clicked(object? sender, EventArgs e)
    {
        var button = (Button)sender!;
        var item = button.BindingContext as Device;
        await _deviceService.DeleteDevice(item!);
        Devices.Remove(item!);
    }

    private async void FiltersButton_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new FiltersPage(_filterParameters,this));
    }

    public async Task ApplyFilters(FilterParameters newFilterParameters)
    {
        _filterParameters = newFilterParameters;
    }
}