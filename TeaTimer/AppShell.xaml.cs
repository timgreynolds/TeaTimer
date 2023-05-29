using com.mahonkin.tim.maui.TeaTimer.Pages;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer;

public partial class AppShell : Shell
{
    public AppShell(TeaNavigationService navigationService)
    {
        Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));

        try
        {
            InitializeComponent();
        }
        catch(System.Exception ex) // Maybe last top-level opportunity to display an exception alert
        {
            this.CurrentPage.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
}