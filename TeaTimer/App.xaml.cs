using System.Reflection;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// <inheritdoc cref="Application" />
/// </summary>
public partial class App : Application
{
    #region Public Properties
    public static readonly string SharedPrefsName = Assembly.GetExecutingAssembly().GetName().Name;
    public static bool UseCelsius;
    public static AppTheme CurrentAppTheme;
    #endregion

    /// <summary>
    /// <inheritdoc cref="Application.Application()" />
    /// </summary>
    public App(AppShell shell)
    {
        InitializeComponent();
        MainPage = shell;
        UseCelsius = Preferences.Get(nameof(UseCelsius), false, SharedPrefsName);
        CurrentAppTheme = (AppTheme)Preferences.Get(nameof(CurrentAppTheme), (int)RequestedTheme, SharedPrefsName);
        UserAppTheme = CurrentAppTheme;
    }
}

