using TrackMe.Models;

namespace TrackMe.Services.Interfaces;

public interface IDBService
{
    Task<int> SaveItemAsync(CustomLocation location);
    Task<List<CustomLocation>> GetTracksAsync();
    Task SaveCurrentTrack(string date, IList<Location> locations);
    Task<List<CustomLocation>> GetTracksFromToday();
}