using System.IO;
using System.Threading.Tasks;
using com.mahonkin.tim.extensions.Logging;
using com.mahonkin.tim.maui.TeaTimer.Utilities;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Foundation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Storage;
using UIKit;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// Maui entry point and application builder.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public static class MauiProgram
{
    public static async Task<MauiApp> CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder()
        .UseMauiApp<TeaTimerApp>()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("SF-Pro.ttf", "SFPro")
                .AddFont("SF-Compact.ttf", "SFCompact")
                .AddFont("SF-Mono-Regular.otf", "SFMono");
        });

#if IOS || MACCATALYST
        builder.ConfigureLifecycleEvents(events =>
            events.AddiOS(ios => ios
                .OnActivated(MacEvents.OnActivatedEvent)
            ));
#endif
        await builder.Configuration.AddAppSettings();

        builder.Services
            .AddPages()
            .AddViewModels()
            .AddServices();

        builder.Logging
            .ClearProviders()
            .AddConfiguration(builder.Configuration.GetSection("Logging"))
#if IOS || MACCATALYST
            .AddUnifiedLogger(opts =>
            {
                if (string.IsNullOrEmpty(NSBundle.MainBundle.BundleIdentifier))
                {
                    opts.Subsystem = typeof(TeaTimerApp).FullName; //NSBundle.MainBundle.PrincipalClass.GetType().FullName;
                }
                else
                {
                    opts.Subsystem = NSBundle.MainBundle.BundleIdentifier;
                }
            })
#endif
            .AddConsole()
            .AddDebug();

        MauiApp app = builder.Build();
        InitDatabase(app.Services.GetRequiredService<IDataService<TeaModel>>());
        return app;
    }

    private static IServiceCollection AddPages(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<Pages.TimerPage>();
        serviceCollection.AddSingleton<Pages.TeaListPage>();
        serviceCollection.AddSingleton<Pages.EditPage>();
        return serviceCollection;
    }

    private static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ViewModels.TimerViewModel>();
        serviceCollection.AddSingleton<ViewModels.TeaListViewModel>();
        serviceCollection.AddSingleton<ViewModels.EditViewModel>();
        return serviceCollection;
    }

    private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IDataService<TeaModel>, TeaSqlService>();
        serviceCollection.AddSingleton<Services.IDisplayService, Services.TeaDisplayService>();
        serviceCollection.AddSingleton<Services.ITimerService, Services.TeaTimerService>();
        serviceCollection.AddSingleton<Services.INavigationService, Services.TeaNavigationService>();
        serviceCollection.AddSingleton<Services.ISettingsService, Services.TeaSettingsService>();
        return serviceCollection;
    }

    private static async Task<IConfigurationBuilder> AddAppSettings(this IConfigurationBuilder builder)
    {
        Stream stream = await FileSystem.Current.OpenAppPackageFileAsync("appsettings.json");
        builder.AddJsonStream(stream);
        return builder;
    }

    private static void InitDatabase(IDataService<TeaModel> sqlService)
    {
        if (FileSystemUtils.AppDataFileExists("kettle.mp3") == false)
        {
            FileSystemUtils.CopyBundleAppDataResource("kettle.mp3");
        }

        string dbFile = "TeaVarieties.db3";
        try
        {
            if (FileSystemUtils.AppDataFileExists(dbFile))
            {
                sqlService.Initialize(FileSystemUtils.GetAppDataFileFullName(dbFile));
            }
            else
            {
                FileSystemUtils.CopyBundleAppDataResource(dbFile);
                sqlService.Initialize(FileSystemUtils.GetAppDataFileFullName(dbFile));
            }
        }
        catch (System.Exception ex)
        {
            throw new System.Exception(ex.Message, ex);
        }
    }
}
