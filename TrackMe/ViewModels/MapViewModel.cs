using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;
using TrackMe.Messages;
using TrackMe.Models;
using TrackMe.Services.Interfaces;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;

namespace TrackMe.ViewModels;

public partial class MapViewModel : BaseViewModel, IDisposable
{

    private readonly ILocationService locationService;
    private readonly IDBService dbService;
    private CustomLocation oldlocation;

    public MapViewModel(ILocationService locationService, IDBService dbService)
    {
        this.locationService = locationService;
        this.locationService.OnLocationUpdate = OnLocationServiceUpdate;
        this.locationService.StartTracking(100);

        this.dbService = dbService;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var list = await dbService.GetTracksFromToday();
            if (list.Count == 0)
                return;
            foreach (var location in list)
            {
                Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
            }
        });
    }

    private void OnLocationServiceUpdate(CustomLocation location)
    {
        if (location.Speed > 0)
        {
            WeakReferenceMessenger.Default.Send(new LocationUpdatedMessage(location));
            Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
            dbService.SaveItemAsync(location);
            
        }
        oldlocation = location;
    }

    private double DistanceBetweenLocations(CustomLocation location)
    {
        return Distance.BetweenPositions(new Location(oldlocation.Latitude, oldlocation.Longitude), new Location(location.Latitude, location.Longitude)).Meters;
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