﻿namespace DeviceControlApp.BLL;

public class FilterParameters
{
    public FilterParameters(string searchField, string sortField, bool sortedByAscendingOrder)
    {
        SearchField = searchField;
        SortField = sortField;
        SortedByAscendingOrder = sortedByAscendingOrder;
    }

    public FilterParameters()
    {
        SearchField = "Name";
        SortField = "Name";
        SortedByAscendingOrder = true;
    }

    public string SearchField { get; set; }
    public string SortField { get; set; }
    public bool SortedByAscendingOrder { get; set; }
}