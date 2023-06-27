using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="Application" />
public partial class App : Application
{
    /// <inheritdoc cref="Application.Application()" />
    public App(INavigationService navigationService)
    {
        InitializeComponent();
        MainPage = new AppShell(navigationService);
        UserAppTheme = AppTheme.Light;
        RequestedThemeChanged += (sender, args) => UserAppTheme = args.RequestedTheme;
    }
}

