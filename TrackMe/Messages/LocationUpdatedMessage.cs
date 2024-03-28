using CommunityToolkit.Mvvm.Messaging.Messages;
using TrackMe.Models;

namespace TrackMe.Messages;

public class LocationUpdatedMessage : ValueChangedMessage<CustomLocation>
{
    public LocationUpdatedMessage(CustomLocation value)
        : base(value)
    {
    }
}