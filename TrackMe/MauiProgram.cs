using Microsoft.Extensions.Logging;
using TrackMe.Services.Interfaces;
using TrackMe.Services;
using TrackMe.ViewModels;
using TrackMe.Views;

namespace TrackMe;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
        => MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }
            )
            .UseMauiMaps()
            .Register()
            .Build();

    public static MauiAppBuilder Register(this MauiAppBuilder mauiAppBuilder)
    {
#if DEBUG
        mauiAppBuilder.Logging.AddDebug();
#endif

        mauiAppBuilder.Services.AddSingleton<ILocationService, LocationService>();
        mauiAppBuilder.Services.AddSingleton<IDBService, DBService>();

        mauiAppBuilder.Services.AddSingleton<MapViewModel>();
        mauiAppBuilder.Services.AddTransient<MapView>();

        return mauiAppBuilder;
    }
}
