using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls.Shapes;
using TrackMe.Messages;
using TrackMe.Models;
using TrackMe.Services.Interfaces;

namespace TrackMe.ViewModels;

public class MapViewModel : BaseViewModel, IDisposable
{


    private readonly ILocationService locationService;
    public RelayCommand SaveTrackCommand { get; private set; }

    public MapViewModel(ILocationService locationService)
    {
        this.locationService = locationService;
        this.locationService.OnLocationUpdate = OnLocationServiceUpdate;
        this.locationService.StartTracking(500);

        SaveTrackCommand = new RelayCommand(() =>
        {
            //
            // Write to text file and share ?
            // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/data/share?view=net-maui-8.0&tabs=windows
            //
        });
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

    private Polyline track;
    public Polyline Track
    {
        get => track;
        set => SetProperty(ref track, value);
    }
}