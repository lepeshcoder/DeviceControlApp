using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceControlApp.BLL;
using DeviceControlApp.BLL.DTO;

namespace DeviceControlApp;

public partial class FiltersPage : ContentPage
{

    private MainPage _mainPage;
    public FilterParametersDto FilterParametersDto { get; set; }
    
    public FiltersPage(FilterParameters filterParameters,MainPage mainPage)
    {  
        FilterParametersDto = new FilterParametersDto
        {
            SearchField = filterParameters.SearchField,
            SortField = filterParameters.SortField,
            SortedByAscendingOrder = filterParameters.SortedByAscendingOrder ? "По возрастанию" : "По убыванию"
        };
        _mainPage = mainPage;
        BindingContext = this;
        InitializeComponent();
    }

    private async void ApplyFilters_Clicked(object? sender, EventArgs e)
    {
        var newFilterParameters = new FilterParameters
        {
            SearchField = FilterParametersDto.SearchField,
            SortField = FilterParametersDto.SortField,
            SortedByAscendingOrder = FilterParametersDto.SortedByAscendingOrder == "По возрастанию" 
        };
        await _mainPage.ApplyFilters(newFilterParameters);
        await Navigation.PopModalAsync();
    }

    private async void BackButton_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}