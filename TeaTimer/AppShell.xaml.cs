using Microsoft.Maui.Controls;
using System;

namespace com.mahonkin.tim.maui.TeaTimer;


public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));
        Routing.RegisterRoute(nameof(TeaList), typeof(TeaList));
    }

    /// <inheritdoc cref="Shell.OnNavigating(ShellNavigatingEventArgs)" />
    //protected override async void OnNavigating(ShellNavigatingEventArgs args)
    //{
    //    base.OnNavigating(args);
    //    try
    //    {
    //        if (args.Current.Location.OriginalString.Contains(nameof(EditPage), StringComparison.InvariantCultureIgnoreCase))
    //        {
    //            await EditPage.OnNavigating(args);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert(ex.GetType().Name, ex.Message, "OK");
    //    }
    //}
}