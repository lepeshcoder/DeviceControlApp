using AutoMapper;
using DeviceControlApp.BLL;
using DeviceControlApp.BLL.MapperProfile;
using DeviceControlApp.DAL.DbContext;
using DeviceControlApp.DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace DeviceControlApp;

//TODO: PUSH-MESSAGES

public static class MauiProgram
{
    public static IServiceProvider? Services;
    public static MauiApp CreateMauiApp()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperProfile>();
        });
        var mapper = mapperConfiguration.CreateMapper();
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
        builder.Services.AddSingleton(mapper);
        Services = builder.Services.BuildServiceProvider();
        
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}