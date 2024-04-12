using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using TrackMe.Helpers;
using TrackMe.Messages;
using TrackMe.ViewModels;

namespace TrackMe.Views;

public partial class MapView : ContentPage
{

    private double mapZoomLevel = 1;

    public MapView(MapViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;

        viewModel.Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 6
        };

        MyMap.MapElements.Add(viewModel.Track);

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var result = await AppPermissions.CheckAndRequestRequiredPermissionAsync();
            if (result != PermissionStatus.Granted)
                return;

            var tmpCurrentLocation = await GetCachedLocation();                
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(tmpCurrentLocation, Distance.FromKilometers(mapZoomLevel));
            MyMap.MoveToRegion(mapSpan);
            viewModel.Track.Geopath.Add(tmpCurrentLocation);

            MyMap.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "VisibleRegion")
                {
                    mapZoomLevel = MyMap.VisibleRegion.Radius.Kilometers;                        
                }                    
            };

            WeakReferenceMessenger.Default.Register<LocationUpdatedMessage>(this, (r, m) =>
            {
                var location = new Location(m.Value.Latitude, m.Value.Longitude);
                if (viewModel.FollowUser)
                {
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, MyMap.VisibleRegion.Radius);
                    MyMap.MoveToRegion(mapSpan);
                }
                viewModel.Track.Geopath.Add(location);
            });            
        });
    }

    public async Task<Location> GetCachedLocation()
    {
        try
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                return location;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }

        return new Location();
    }
}