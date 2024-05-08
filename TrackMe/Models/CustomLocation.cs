namespace TrackMe.Models;

public class CustomLocation
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Date { get; set; }
    public double? Speed { get; set; }

    public CustomLocation()
    {        
    }

    public CustomLocation(double latitude, double longitude, string date, double? speed)
    {
        Latitude = latitude;
        Longitude = longitude;
        Date = date;
        Speed = speed;
    }

    public override string ToString()
    {
        return $"Latitude: {Latitude} Longitude: {Longitude} Date: {Date} Speed: {Speed}";
    }
}
