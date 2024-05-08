using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using System.Diagnostics;
using TrackMe.Services.Interfaces;

namespace TrackMe.ViewModels;

public partial class HistoryViewModel : ObservableObject
{

    private readonly IDBService dbService;

    public HistoryViewModel(IDBService dbService)
    {
        this.dbService = dbService;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var list = await dbService.GetTracksFromToday();
            foreach (var location in list)
            {
                Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
            }
        });
    }

    [RelayCommand]
    private async Task PreviousDay()
    {
        var list = await dbService.GetTracksFromPreviousDay();
        Track.Geopath.Clear();
        foreach (var location in list)
        {
            Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
        }
    }

    [RelayCommand]
    private async Task NextDay()
    {
        var list = await dbService.GetTracksFromNextDay();
        Track.Geopath.Clear();
        foreach (var location in list)
        {
            Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
        }
    }

    [ObservableProperty]
    private Polyline track;
}