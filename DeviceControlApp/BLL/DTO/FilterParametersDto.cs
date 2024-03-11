using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DeviceControlApp.BLL.DTO;

public class FilterParametersDto : INotifyPropertyChanged
{

    private string _searchField;
    private string _sortField;
    private string _sortedByAscendingOrder;

    [Required(ErrorMessage = "SearchField is required")]
    public string SearchField
    {
        get => _searchField;
        set => SetField(ref _searchField, value);
    }

    [Required(ErrorMessage = "SortField is required")]
    public string SortField
    {
        get => _sortField;
        set => SetField(ref _sortField, value);
    }

    [Required(ErrorMessage = "SortedByAscendingOrder is required")]
    public string SortedByAscendingOrder
    {
        get => _sortedByAscendingOrder;
        set => SetField(ref _sortedByAscendingOrder, value);
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