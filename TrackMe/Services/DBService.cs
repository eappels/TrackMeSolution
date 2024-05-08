using SQLite;
using TrackMe.Helpers;
using TrackMe.Models;
using TrackMe.Services.Interfaces;

namespace TrackMe.Services;

public class DBService : IDBService
{

    private SQLiteAsyncConnection Database;
    private DateTime LastDate;

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
        LastDate = DateTime.Now;
        return await Database.Table<CustomLocation>().Where(x => x.Date.Contains(LastDate.ToString("dd-MM-yyyy"))).ToListAsync();
    }

    public async Task<List<CustomLocation>> GetTracksFromPreviousDay()
    {
        await Init();
        LastDate = LastDate.AddDays(-1);
        return await Database.Table<CustomLocation>().Where(x => x.Date.Contains(LastDate.ToString("dd-MM-yyyy"))).ToListAsync();
    }

    public async Task<List<CustomLocation>> GetTracksFromNextDay()
    {
        await Init();
        LastDate = LastDate.AddDays(1);
        return await Database.Table<CustomLocation>().Where(x => x.Date.Contains(LastDate.ToString("dd-MM-yyyy"))).ToListAsync();
    }

    public async Task DeleteAllTracksAsync()
    {
        await Init();
        await Database.DropTableAsync<CustomLocation>();
        await Database.CloseAsync();
    }
}