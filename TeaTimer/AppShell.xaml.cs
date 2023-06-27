using com.mahonkin.tim.maui.TeaTimer.Pages;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="Shell"/>
public partial class AppShell : Shell
{
    /// <inheritdoc cref="TeaNavigationService" />
    public AppShell(INavigationService navigationService)
    {
        Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));

        try
        {
            InitializeComponent();
        }
        catch (System.Exception ex) // Maybe last top-level opportunity to display an exception alert
        {
            CurrentPage.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
}