using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Pages;

/// <inheritdoc cref="ContentPage" />
public partial class SettingsPage : ContentPage
{
    #region Constructors
    public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
    #endregion Constructors
}
