﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using DeviceControlApp.DAL.ValidationAttributes;

namespace DeviceControlApp.BLL.DTO;

public class DeviceEditDto : INotifyPropertyChanged
{
    private string _name;
    private string? _description;
    private DateTime? _lastVerificationTime;
    private DateTime? _nextVerificationTime;
    private string _factoryNumber;
    private string _inventoryNumber;
    private string _owner;


    [Required(ErrorMessage = "Device Name is required")]
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string? Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public DateTime? LastVerificationTime
    {
        get => _lastVerificationTime;
        set => SetField(ref _lastVerificationTime, value);
    }

    [DateCannotBeInPast(ErrorMessage = "Last Verification Time cannot be in the past")]
    public DateTime? NextVerificationTime
    {
        get => _nextVerificationTime;
        set => SetField(ref _nextVerificationTime, value);
    }

    public string FactoryNumber
    {
        get => _factoryNumber;
        set => SetField(ref _factoryNumber, value);
    }

    public string InventoryNumber
    {
        get => _inventoryNumber;
        set => SetField(ref _inventoryNumber, value);
    }

    public string Owner
    {
        get => _owner;
        set => SetField(ref _owner, value);
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}