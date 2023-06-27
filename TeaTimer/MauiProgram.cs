using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// Maui entry point and application builder.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public static class MauiProgram
{
    /// <inheritdoc cref="MauiApp.CreateBuilder(bool)" />
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder()
        .UseMauiApp<App>()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        });

        builder.Services.AddPages();
        builder.Services.AddViewModels();
        builder.Services.AddServices();

        return builder.Build();
    }

    private static IServiceCollection AddPages(this IServiceCollection pages)
    {
        pages.AddSingleton<Pages.TimerPage>();
        pages.AddSingleton<Pages.TeaListPage>();
        pages.AddSingleton<Pages.EditPage>();
        return pages;
    }

    private static IServiceCollection AddViewModels(this IServiceCollection viewModels)
    {
        viewModels.AddSingleton<ViewModels.TimerViewModel>();
        viewModels.AddSingleton<ViewModels.TeaListViewModel>();
        viewModels.AddSingleton<ViewModels.EditViewModel>();
        return viewModels;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<Services.IDataService<DataModel.TeaModel>, Services.TeaSqlService<DataModel.TeaModel>>();
        services.AddSingleton<Services.IDisplayService, Services.TeaDisplayService>();
        services.AddSingleton<Services.ITimerService, Services.TeaTimerService>();
        services.AddSingleton<Services.INavigationService, Services.TeaNavigationService>();
        return services;
    }
}
