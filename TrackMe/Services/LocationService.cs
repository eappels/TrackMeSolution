using TrackMe.Models;
using TrackMe.Services.Interfaces;

namespace TrackMe.Services;

public partial class LocationService : ILocationService
{

    public Action<CustomLocation> OnLocationUpdate { get; set; }

    public void StartTracking(double distanceFilter = 250)
    {
        StartTrackingInternal(distanceFilter);
    }

    public void StopTracking()
    {
        StopTrackingInternal();
    }

    partial void StartTrackingInternal(double distanceFilter);
    partial void StopTrackingInternal();

}