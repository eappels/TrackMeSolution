using CoreLocation;
using TrackMe.Models;

namespace TrackMe.Services;

public partial class LocationService
{

    public readonly CLLocationManager locationManager;

    public LocationService()
    {
        locationManager = new CLLocationManager();
        locationManager.PausesLocationUpdatesAutomatically = false;
        locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
        locationManager.AllowsBackgroundLocationUpdates = true;
        locationManager.ActivityType = CLActivityType.AutomotiveNavigation;        
    }

    partial void StartTrackingInternal(double distanceFilter)
    {
        locationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
        {
            CustomLocation newLocation = new CustomLocation(e.Locations.LastOrDefault().Coordinate.Latitude, e.Locations.LastOrDefault().Coordinate.Longitude);
            OnLocationUpdate?.Invoke(newLocation);
        };
        locationManager.DistanceFilter = distanceFilter;
        locationManager.StartUpdatingLocation();
    }

    partial void StopTrackingInternal()
    {
        locationManager.StopUpdatingLocation();
    }
}