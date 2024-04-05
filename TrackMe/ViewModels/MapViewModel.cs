using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using TrackMe.Messages;
using TrackMe.Models;
using TrackMe.Services.Interfaces;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;

namespace TrackMe.ViewModels;

public partial class MapViewModel : BaseViewModel, IDisposable
{


    private readonly ILocationService locationService;
    private readonly IDBService dbService;

    public MapViewModel(ILocationService locationService, IDBService dbService)
    {
        this.locationService = locationService;
        this.locationService.OnLocationUpdate = OnLocationServiceUpdate;
        this.locationService.StartTracking(100);

        this.dbService = dbService;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var list = await dbService.GetTracksFromToday();
            foreach (var location in list)
            {
                track.Geopath.Add(new Location(location.Latitude, location.Longitude));
            }
        });        
    }

    private void OnLocationServiceUpdate(CustomLocation location)
    {
        WeakReferenceMessenger.Default.Send(new LocationUpdatedMessage(location));
        track.Geopath.Add(new Location(location.Latitude, location.Longitude));
        dbService.SaveItemAsync(location);
    }

    public void Dispose()
    {
        if (locationService != null)
        {
            locationService.StopTracking();
            locationService.OnLocationUpdate -= OnLocationServiceUpdate;
        }

    }

    [RelayCommand]
    public async Task SaveTrack()
    {
        locationService.OnLocationUpdate -= OnLocationServiceUpdate;
        var date = DateTime.Now.ToString("dd-MM-yyyy-HHmmss");
        string fileLocation = Path.Combine(FileSystem.AppDataDirectory, date + ".txt");
        await dbService.SaveCurrentTrack(date, track.Geopath);
        using (StreamWriter outFile = new StreamWriter(fileLocation))
        {
            foreach (Location location in track.Geopath)
            {
                CustomLocation customLocation = new CustomLocation(location.Latitude, location.Longitude, date);
                outFile.WriteLine(customLocation.ToString());
            }
        }

        await ShareFile(fileLocation);
    }    

    public async Task ShareFile(string file)
    {
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "TrackMe - Saved track",
            File = new ShareFile(file)
        });
        if (File.Exists(file))
        {
            File.Delete(file);
        }
        track.Geopath.Clear();
        locationService.OnLocationUpdate += OnLocationServiceUpdate;
    }

    private Polyline track;
    public Polyline Track
    {
        get => track;
        set => SetProperty(ref track, value);
    }

    private bool followUser;
    public bool FollowUser
    {
        get => followUser;
        set => SetProperty(ref followUser, value);
    }
}