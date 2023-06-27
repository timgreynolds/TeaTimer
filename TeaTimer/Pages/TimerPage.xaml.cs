using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Pages;

/// <inheritdoc cref="ContentPage" />
public partial class TimerPage : ContentPage
{
	/// <inheritdoc cref="ContentPage()" />
	public TimerPage(TimerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
