using System;
using System.Collections.Generic;
using com.mahonkin.tim.maui.TeaTimer.DataModel;
using com.mahonkin.tim.maui.TeaTimer.Pages;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer;

/// <inheritdoc cref="ContentPage" />
public partial class TeaList : ContentPage
{
    public TeaList()
    {
        InitializeComponent();
        //TeaListView.ItemsSource = TeaModel.Teas;
        //TeaListView.SelectedItem = null;
    }

    /// <summary>
    /// Navigate to the Add/Edit Tea page.
    /// </summary>
    /// <param name="sender">The object in the page that the user tapped or clicked.</param>
    /// <param name="e">Event arguments passed on a tapped event.</param>
    //async public void TeaListView_ItemTapped(Object sender, EventArgs e)
    //{
    //    if (sender is Button)
    //    {
    //        try
    //        {
    //            await Shell.Current.GoToAsync(nameof(EditPage), true);
    //        }
    //        catch(Exception ex)
    //        {
    //            await DisplayAlert(ex.GetType().Name, ex.Message, "OK");
    //        }
    //    }
    //    else if (sender is ListView)
    //    {
    //        ItemTappedEventArgs args = e as ItemTappedEventArgs;
    //        TeaModel selectedTea = args.Item as TeaModel;
    //        try
    //        {
    //            await Shell.Current.GoToAsync(nameof(EditPage), true, new Dictionary<string, object> { ["tea"] = selectedTea } );
    //        }
    //        catch (Exception ex)
    //        {
    //            await DisplayAlert(ex.GetType().Name, ex.Message, "OK");
    //        }
    //    }
    //}
}