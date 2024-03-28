using CommunityToolkit.Mvvm.Messaging;
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

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var result = await AppPermissions.CheckAndRequestRequiredPermissionAsync();

            if (result == PermissionStatus.Granted)
            {
                var tmpCurrentLocation = await GetCachedLocation();
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(tmpCurrentLocation, Distance.FromKilometers(mapZoomLevel));
                MyMap.MoveToRegion(mapSpan);


                MyMap.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "VisibleRegion")
                    {
                        mapZoomLevel = MyMap.VisibleRegion.Radius.Kilometers;
                    }
                };

                WeakReferenceMessenger.Default.Register<LocationUpdatedMessage>(this, (r, m) =>
                {
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(new Location(m.Value.Latitude, m.Value.Longitude), Distance.FromKilometers(mapZoomLevel));
                    MyMap.MoveToRegion(mapSpan);
                });
            }
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