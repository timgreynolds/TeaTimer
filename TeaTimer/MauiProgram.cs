using System.Collections.Generic;
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
using UIKit;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// Maui entry point and application builder.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public static class MauiProgram
{
    private static ILogger _logger;

    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder()
        .UseMauiApp<TeaTimerApp>()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular")
                 .AddFont("OpenSans-Semibold.ttf", "OpenSansSemiBold")
                 .AddFont("Stencil.ttf", "Stencil");
        })
        .ConfigureLifecycleEvents(events =>
        {
#if IOS || MACCATALYST
            events.AddiOS(ios => ios
                .OnActivated(MacEvents.OnActivatedEvent)
            );
#endif
        });

        builder.Configuration.AddAppSettings();

        builder.Services
            .AddPages()
            .AddViewModels()
            .AddServices();

        builder.Logging
            .ClearProviders()
            .AddConfiguration(builder.Configuration.GetSection("Logging"))
#if IOS || MACCATALYST
            .AddUnifiedLogger(o =>
            {
                o.Subsystem = NSBundle.MainBundle.BundleIdentifier;
            })
#endif
            .AddConsole()
            .AddDebug();

        MauiApp app = builder.Build();
        _logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(MauiProgram));
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

    private static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder builder)
    {
        if (FileSystemUtils.AppDataFileExists("appsettings.json") == false)
        {
            FileSystemUtils.CopyBundleAppDataResource("appsettings.json");
        }
        builder.AddJsonFile(FileSystemUtils.GetAppDataFileFullName("appsettings.json"));
        
        return builder;
    }

    private static void InitDatabase(IDataService<TeaModel> sqlService)
    {
        if (FileSystemUtils.AppDataFileExists("kettle.mp3") == false)
        {
            _logger.LogDebug("Copying kettle whistle sound file from the bundle to the device.");
            FileSystemUtils.CopyBundleAppDataResource("kettle.mp3");
        }

        string dbFile = "TeaVarieties.db3";
        _logger.LogDebug("Database initialization using {FileName}.", dbFile);
        try
        {
            if (FileSystemUtils.AppDataFileExists(dbFile))
            {
                _logger.LogDebug("Found database {FileName} file.", dbFile);
                sqlService.Initialize(FileSystemUtils.GetAppDataFileFullName(dbFile));
            }
            else
            {
                _logger.LogDebug("Did not find {FileName} file. Copying from the bundle to the device.", dbFile);
                FileSystemUtils.CopyBundleAppDataResource(dbFile);
                sqlService.Initialize(FileSystemUtils.GetAppDataFileFullName(dbFile));
            }

            List<TeaModel> teas = sqlService.Get();
            if (teas.Count < 1)
            {
                _logger.LogInformation("No teas found in the tea database.");
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogCritical("An exception occurred. {Type} - {Message}", ex.GetType().Name, ex.Message);
        }
    }
}
