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

    public MapViewModel(ILocationService locationService)
    {
        this.locationService = locationService;
        this.locationService.OnLocationUpdate = OnLocationServiceUpdate;
        this.locationService.StartTracking(250);
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

    [RelayCommand]
    public void SaveTrack()
    {
        // Write to text file and share ?
        // https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/data/share?view=net-maui-8.0&tabs=windows
    }

    private Polyline track;
    public Polyline Track
    {
        get => track;
        set => SetProperty(ref track, value);
    }

    private bool isToggled;
    public bool IsToggled
    {
        get => isToggled;
        set => SetProperty(ref isToggled, value);
    }
}