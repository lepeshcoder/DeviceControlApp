using DeviceControlApp.BLL;
using DeviceControlApp.DAL.DbContext;
using DeviceControlApp.DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace DeviceControlApp;

public static class MauiProgram
{
    public static IServiceProvider? Services;
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddScoped<DeviceRepository>();
        builder.Services.AddScoped<DeviceService>();
        Services = builder.Services.BuildServiceProvider();
        
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}