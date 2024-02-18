using com.mahonkin.tim.maui.TeaTimer.Utilities;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using com.mahonkin.tim.TeaDataService.Services.TeaSqLiteService;
using com.mahonkin.tim.UnifiedLogging.Extensions;
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
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder()
        .UseMauiApp<TeaTimerApp>()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        })
        .ConfigureLifecycleEvents(events =>
        {
#if IOS || MACCATALYST
            events.AddiOS(ios => ios
                .OnActivated(MacEvents.OnActivatedEvent)
            );
#endif
        });

        builder.Services
            .AddPages()
            .AddViewModels()
            .AddServices();

        builder.Logging
            .ClearProviders()
            .SetMinimumLevel(LogLevel.Debug)
#if IOS || MACCATALYST
            .AddUnifiedLogger()
#endif
            .AddDebug();

        MauiApp app = builder.Build();
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
