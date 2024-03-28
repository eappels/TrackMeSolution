using CoreLocation;
using TrackMe.Models;

namespace TrackMe.Services;

public partial class LocationService
{

    private CLLocationManager locationManager;

    public LocationService()
    {

    }

    partial void StartTrackingInternal(double distanceFilter)
    {

#if DEBUG
        distanceFilter = 0.1;
#endif
        locationManager = new CLLocationManager
        {
            PausesLocationUpdatesAutomatically = false,
            DesiredAccuracy = CLLocation.AccurracyBestForNavigation,
            AllowsBackgroundLocationUpdates = true,
            ActivityType = CLActivityType.AutomotiveNavigation,
            DistanceFilter = distanceFilter
        };

        locationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
        {
            CustomLocation newLocation = new CustomLocation(e.Locations.LastOrDefault().Coordinate.Latitude, e.Locations.LastOrDefault().Coordinate.Longitude);
            OnLocationUpdate?.Invoke(newLocation);
        };

        locationManager.StartUpdatingLocation();
    }

    partial void StopTrackingInternal()
    {
        locationManager.StopUpdatingLocation();
    }
}