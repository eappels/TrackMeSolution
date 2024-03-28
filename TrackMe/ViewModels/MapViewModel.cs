using CommunityToolkit.Mvvm.Messaging;
using TrackMe.Messages;
using TrackMe.Models;
using TrackMe.Services.Interfaces;

namespace TrackMe.ViewModels;

public class MapViewModel : BaseViewModel, IDisposable
{


    private readonly ILocationService locationService;

    public MapViewModel(ILocationService locationService)
    {
        this.locationService = locationService;
        this.locationService.OnLocationUpdate = OnLocationServiceUpdate;
        this.locationService.StartTracking(1);
    }

    private void OnLocationServiceUpdate(CustomLocation location)
    {
        WeakReferenceMessenger.Default.Send(new LocationUpdatedMessage(location));
    }

    public void Dispose()
    {
        if (locationService != null)
        {
            locationService.StopTracking();
            locationService.OnLocationUpdate -= OnLocationServiceUpdate;
        }

    }
}