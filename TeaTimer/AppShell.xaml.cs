using com.mahonkin.tim.maui.TeaTimer.Pages;
using com.mahonkin.tim.maui.TeaTimer.Services;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer;


public partial class AppShell : Shell
{
    private readonly INavigationService _navigationService;

    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;

        Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));

        InitializeComponent();
    }
}