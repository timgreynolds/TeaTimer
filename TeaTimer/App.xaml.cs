using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using System.Reflection;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <summary>
/// <inheritdoc cref="Microsoft.Maui.Controls.Application" />
/// </summary>
public partial class App : Application
{
    #region Public Properties
    public static readonly string SharedPrefsName = Assembly.GetExecutingAssembly().GetName().Name;
    public static bool UseCelsius;
    public static AppTheme CurrentAppTheme;
    #endregion

    /// <summary>
    /// <inheritdoc cref="Microsoft.Maui.Controls.Application.Application()" />
    /// </summary>
    public App()
	{
		InitializeComponent();
		MainPage = new AppShell();
        UseCelsius = Preferences.Get(nameof(UseCelsius), false, SharedPrefsName);
        CurrentAppTheme = (AppTheme)Preferences.Get(nameof(CurrentAppTheme), (int)RequestedTheme, SharedPrefsName);
        UserAppTheme = CurrentAppTheme;
	}
}

