using TrackMe.Models;

namespace TrackMe.Services.Interfaces;

public interface ILocationService
{
    Action<CustomLocation> OnLocationUpdate { get; set; }
    void StartTracking(double distanceFilter);
    void StopTracking();
}