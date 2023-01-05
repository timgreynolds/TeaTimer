using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="Microsoft.Maui.Hosting.MauiApp" />
public static class MauiProgram
{
    /// <inheritdoc cref="Microsoft.Maui.Hosting.MauiApp.CreateBuilder(bool)" />
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
        .UseMauiApp<App>()
        .UseMauiCommunityToolkit()
        .UseMauiCommunityToolkitMarkup()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        });
        return builder.Build();
    }
}
