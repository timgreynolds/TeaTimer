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

        builder.Services.AddSingleton<Pages.TimerPage>();
        builder.Services.AddTransient<Pages.TeaListPage>();
        builder.Services.AddTransient<Pages.EditPage>();

        builder.Services.AddSingleton<ViewModels.TimerViewModel>();
        builder.Services.AddTransient<ViewModels.TeaListViewModel>();
        builder.Services.AddTransient<ViewModels.EditViewModel>();

        builder.Services.AddSingleton<Services.TeaNavigationService, Services.TeaNavigationService>();
        builder.Services.AddSingleton<Services.TeaDisplayService, Services.TeaDisplayService>();
        builder.Services.AddSingleton<Services.TeaTimerService, Services.TeaTimerService>();
        builder.Services.AddSingleton<Services.TeaSqlService, Services.TeaSqlService>();

        return builder.Build();
    }
}
