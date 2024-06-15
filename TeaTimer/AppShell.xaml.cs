using com.mahonkin.tim.maui.TeaTimer.Pages;
using com.mahonkin.tim.maui.TeaTimer.Services;
using com.mahonkin.tim.TeaDataService.DataModel;
using com.mahonkin.tim.TeaDataService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="Shell"/>
public partial class AppShell : Shell
{
    /// <inheritdoc cref="Shell()" />
    public AppShell(INavigationService navigationService, IDataService<TeaModel> sqlService, ISettingsService settingsService, ILoggerFactory loggerFactory)
    {
        loggerFactory.CreateLogger<AppShell>().LogTrace("Constructor entered.");

        Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));
        InitializeComponent();
    }
}