using SQLite;
using TrackMe.Helpers;
using TrackMe.Models;
using TrackMe.Services.Interfaces;

namespace TrackMe.Services;

public class DBService : IDBService
{

    private SQLiteAsyncConnection Database;

    public DBService()
    {        
    }

    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        var result = await Database.CreateTableAsync<CustomLocation>();
    }

    public async Task<int> SaveItemAsync(CustomLocation location)
    {
        await Init();
        return await Database.InsertAsync(location);
    }

    public async Task<List<CustomLocation>> GetTracksAsync()
    {
        await Init();
        return await Database.Table<CustomLocation>().ToListAsync();
    }

    public async Task<List<CustomLocation>> GetTracksFromToday()
    {
        await Init();
        return await Database.Table<CustomLocation>().Where(x => x.Date.Contains(DateTime.Now.ToString("dd-MM-yyyy"))).ToListAsync();
    }

    public async Task SaveCurrentTrack(string date, IList<Location> locations)
    {
        var customLocations = new List<CustomLocation>();
        foreach (var loc in locations)
        {
            customLocations.Add(new CustomLocation(loc.Latitude, loc.Longitude, date));
        }
        await Init();
        await Database.InsertAllAsync(customLocations);
    }
}