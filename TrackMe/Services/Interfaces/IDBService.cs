using TrackMe.Models;

namespace TrackMe.Services.Interfaces;

public interface IDBService
{
    Task<int> SaveItemAsync(CustomLocation location);
    Task<List<CustomLocation>> GetTracksAsync();
    Task<List<CustomLocation>> GetTracksFromToday();
    Task<List<CustomLocation>> GetTracksFromPreviousDay();
    Task<List<CustomLocation>> GetTracksFromNextDay();
}