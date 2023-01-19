using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="MauiApp" />
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

        //builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<Pages.TimerPage>();
        builder.Services.AddSingleton<Pages.TeaListPage>();
        builder.Services.AddSingleton<Pages.EditPage>();
        builder.Services.AddSingleton<ViewModels.TimerViewModel>();
        builder.Services.AddSingleton<ViewModels.TeaListViewModel>();
        builder.Services.AddSingleton<ViewModels.EditViewModel>();

        builder.Services.AddSingleton<Services.INavigationService, Services.TeaNavigationService>();
        builder.Services.AddSingleton<Services.IDisplayService, Services.DisplayService>();

        return builder.Build();
    }
}
