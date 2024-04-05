namespace TrackMe.Models;

public class CustomLocation
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Date { get; set; }

    public CustomLocation()
    {        
    }

    public CustomLocation(double latitude, double longitude, string date)
    {
        Latitude = latitude;
        Longitude = longitude;
        Date = date;
    }

    public override string ToString()
    {
        return $"{Latitude} {Longitude} {Date}";
    }
}
