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

        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<Pages.TimerPage>();
        builder.Services.AddSingleton<ViewModels.TimerViewModel>();
        return builder.Build();
    }
}
