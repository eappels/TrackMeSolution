using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using TrackMe.Messages;
using TrackMe.Models;
using TrackMe.Services.Interfaces;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;

namespace TrackMe.ViewModels;

public partial class MapViewModel : BaseViewModel, IDisposable
{

    private readonly ILocationService locationService;
    private readonly IDBService dbService;

    public MapViewModel(ILocationService locationService, IDBService dbService)
    {
        this.locationService = locationService;
        this.locationService.OnLocationUpdate = OnLocationServiceUpdate;
        this.locationService.StartTracking(100);

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

    private void OnLocationServiceUpdate(CustomLocation location)
    {
        WeakReferenceMessenger.Default.Send(new LocationUpdatedMessage(location));
        Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
        dbService.SaveItemAsync(location);
    }

    public void Dispose()
    {
        if (locationService != null)
        {
            locationService.StopTracking();
            locationService.OnLocationUpdate -= OnLocationServiceUpdate;
        }

    }

    [ObservableProperty]
    private Polyline track;    

    [ObservableProperty]
    private bool followUser;
}