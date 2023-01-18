using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Pages;

/// <inheritdoc cref="ContentPage" />
public partial class EditPage : ContentPage //, IQueryAttributable
{
    //#region Private Fields
    //private TeaModel _tea = null;
    //private bool _pageIsDirty = false;
    //#endregion Private Fields

    #region Constructors
    /// <summary>
    /// EditPage constructor.
    /// </summary>
    public EditPage(EditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        //Shell.SetBackButtonBehavior(this, new BackButtonBehavior { Command = new Command(async () => await BackBtnCmd()) });
    }
    #endregion Constructors

    //#region Private Methods
    //async private void SaveBtn_Clicked(Object sender, EventArgs e)
    //{
    //    if (_tea.Id > 0) // This is an existing tea variety to be updated
    //    {
    //        try
    //        {
    //            if (await TeaModel.UpdateAsync(_tea))
    //            {
    //                _pageIsDirty = false;
    //                SaveBtn.IsEnabled = false;
    //                await DisplayAlert("Tea Updated!", "The selected tea was succesfully updated.", "OK");
    //                await Shell.Current.GoToAsync("..", true);
    //            }
    //            else
    //            {
    //                _pageIsDirty = true;
    //                await DisplayAlert("Update Failed!", "An error ocurred trying to update the tea variety.", "Cancel");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await DisplayAlert("Update Failed! An exception was thrown.", ex.Message, "Cancel");
    //        }
    //    }
    //    else // This is a new tea variety
    //    {
    //        try
    //        {
    //            _tea = await TeaModel.AddAsync(_tea);
    //            if (_tea.Id > 0)
    //            {
    //                _pageIsDirty = false;
    //                SaveBtn.IsEnabled = false;
    //                await DisplayAlert("Tea Added!", "The selected tea was succesfully added.", "OK");
    //                await Shell.Current.GoToAsync("..", true);
    //            }
    //            else
    //            {
    //                _pageIsDirty = true;
    //                await DisplayAlert("Add Failed!", "An error ocurred trying to add the tea variety", "Cancel");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await DisplayAlert("Add Failed! An exception was thrown.", ex.Message, "Cancel");
    //        }
    //    }
    //}

    //private void TeaProperty_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    if (e.OldTextValue != null) //If OldTextValue is null this is page initialization; don't do this.
    //    {
    //        _pageIsDirty = true;
    //        SaveBtn.IsEnabled = true;
    //    }
    //}

    //private async Task BackBtnCmd()
    //{
    //    bool abandon = true;
    //    if (_pageIsDirty)
    //    {
    //        try
    //        {
    //            abandon = await Shell.Current.DisplayAlert("Warning!", "There are unsaved changes on the page.", "Abandon Changes", "Return to Editing");
    //        }
    //        catch (Exception ex)
    //        {
    //            await Shell.Current.DisplayAlert(ex.GetType().Name, ex.Message, "OK");
    //        }
    //    }
    //    if (abandon)
    //    {
    //        await Shell.Current.GoToAsync("..");
    //    }
    //}
    //#endregion Private Methods

    ///// <inheritdoc cref="IQueryAttributable.ApplyQueryAttributes(IDictionary{string, object})"/>
    //void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    //{
    //    if(_tea is not null)
    //    {
    //        _tea = null;
    //    }
    //    object obj;
    //    if (query.TryGetValue("tea", out obj))
    //    {
    //        _tea = obj as TeaModel;
    //    }
    //    else
    //    {
    //        _tea = new TeaModel(string.Empty);
    //    }
    //    BindingContext = _tea;
    //    SaveBtn.IsEnabled = false;
    //    _pageIsDirty = false;
    //}
}
