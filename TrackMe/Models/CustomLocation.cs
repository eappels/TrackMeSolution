namespace TrackMe.Models;

public class CustomLocation
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public CustomLocation()
    {        
    }

    public CustomLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public override string ToString()
    {
        return $"{Latitude} {Longitude}";
    }
}
