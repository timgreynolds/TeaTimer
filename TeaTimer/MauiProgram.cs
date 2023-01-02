using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace com.mahonkin.tim.maui.TeaTimer;
/// <summary>
/// MauiProgram application entry point
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Create the App
    /// </summary>
    /// <returns>The generated MauiApp</returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
        .UseMauiApp<App>()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        });
        return builder.Build();
    }
}
