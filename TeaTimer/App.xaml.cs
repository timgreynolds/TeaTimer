using System.Reflection;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="Application" />
public partial class App : Application
{
    /// <inheritdoc cref="Application.Application()" />
    public App(TeaNavigationService navigationService)
    {
        InitializeComponent();
        MainPage = new AppShell(navigationService);
        UserAppTheme = AppTheme.Light;
        RequestedThemeChanged += (sender, args) => UserAppTheme = args.RequestedTheme;
    }
}

