using com.mahonkin.tim.logging.UnifiedLogging.Extensions;
using com.mahonkin.tim.maui.TeaTimer.Utilities;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using com.mahonkin.tim.TeaDataService.Services.TeaSqLiteService;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;
using Microsoft.Maui.LifecycleEvents;
using UIKit;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// Maui entry point and application builder.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public static class MauiProgram
{
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

        if (FileSystem.Current.AppPackageFileExistsAsync("appsettings.json").Result)
        {
            using Stream settingsStream = FileSystem.Current.OpenAppPackageFileAsync("appsettings.json").Result;
            builder.Configuration.AddJsonStream(settingsStream);
        }

        builder.Services
            .AddPages()
            .AddViewModels()
            .AddServices();

        builder.Logging
            .ClearProviders()
#if IOS || MACCATALYST
            .AddUnifiedLogger(options =>
            {
                options.Subsystem = NSBundle.MainBundle.BundleIdentifier;
            })
#endif
            .AddConsole()
            .AddDebug();

        MauiApp app = builder.Build();

        ILogger logger = app.Services.GetRequiredService<ILogger>();
        logger.LogDebug("Application built, will run.");
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
}
